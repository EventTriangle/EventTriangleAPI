using System.IdentityModel.Tokens.Jwt;
using EventTriangleAPI.Authorization.Domain.Constants;
using EventTriangleAPI.Authorization.Domain.Entities;
using EventTriangleAPI.Authorization.Domain.Exceptions;
using EventTriangleAPI.Authorization.Persistence;
using EventTriangleAPI.Shared.Application.Constants;
using EventTriangleAPI.Shared.DTO.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using static IdentityModel.OidcConstants;
using TokenResponse = IdentityModel.Client.TokenResponse;

namespace EventTriangleAPI.Authorization.BusinessLogic.Services;

public class TicketStore : ITicketStore
{
    private readonly DatabaseContext _context;
    private readonly TicketSerializer _ticketSerializer;
    private readonly HttpClient _httpClient;
    private readonly AzureAdConfiguration _azureAdConfiguration;
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;

    public TicketStore(
        DatabaseContext context,
        TicketSerializer ticketSerializer,
        HttpClient httpClient,
        AzureAdConfiguration azureAdConfiguration,
        IMemoryCache memoryCache)
    {
        _context = context;
        _ticketSerializer = ticketSerializer;
        _httpClient = httpClient;
        _azureAdConfiguration = azureAdConfiguration;
        _memoryCache = memoryCache;

        _memoryCacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(15))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(300));
    }

    public async Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        var idToken = ticket.Properties.GetTokenValue(TokenTypes.IdentityToken);
        var accessToken = ticket.Properties.GetTokenValue(TokenTypes.AccessToken);
        var refreshToken = ticket.Properties.GetTokenValue(TokenTypes.RefreshToken);
        
        var handler = new JwtSecurityTokenHandler();
        var decodeToken = handler.ReadToken(idToken) as JwtSecurityToken;
        var ticketExpiresUtc = ticket.Properties.ExpiresUtc;
        
        if (decodeToken == null)
        {
            throw new StoreException("Read token error");
        }
        
        if (ticketExpiresUtc.HasValue == false)
        {
            throw new StoreException("Ticket ExpiresUtc value does not exist");
        }
        
        var sessionId = decodeToken.Claims.First(x => x.Type == ClaimsConstants.Sid).Value;
        var userSession = await _context.UserSessions.FirstOrDefaultAsync();

        if (userSession != null)
        {
            var deserializedTicket = _ticketSerializer.Deserialize(userSession.Value);
            
            if (deserializedTicket == null)
            {
                throw new StoreException("Deserialization ticket error");
            }
            
            if (accessToken == null || refreshToken == null || idToken == null)
            {
                throw new StoreException("Access token, refresh token, identity token are not existing");
            }
            
            deserializedTicket.Properties.UpdateTokenValue(TokenTypes.AccessToken, accessToken);
            deserializedTicket.Properties.UpdateTokenValue(TokenTypes.RefreshToken, refreshToken);
            deserializedTicket.Properties.UpdateTokenValue(TokenTypes.IdentityToken, idToken);

            var serializedTicket = _ticketSerializer.Serialize(deserializedTicket);

            userSession.UpdateValue(serializedTicket);
            userSession.UpdateExpiresAt(ticketExpiresUtc.Value);

            _context.UserSessions.Update(userSession);
            await _context.SaveChangesAsync();
            
            _memoryCache.Set(sessionId, ticket, _memoryCacheEntryOptions);
        }
        
        if (userSession == null)
        {
            var serializedTicket = _ticketSerializer.Serialize(ticket);
            
            var newUserSession = new UserSessionEntity(new Guid(sessionId), ticketExpiresUtc.Value, serializedTicket);

            _context.UserSessions.Add(newUserSession);
            await _context.SaveChangesAsync();
            
            _memoryCache.Set(sessionId, ticket, _memoryCacheEntryOptions);
        }
        
        return sessionId;
    }

    public async Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        var ticketExpiresUtc = ticket.Properties.ExpiresUtc;

        if (ticketExpiresUtc.HasValue == false)
        {
            throw new StoreException("Ticket ExpiresUtc value does not exist");
        }
        
        if (DateTimeOffset.UtcNow < ticketExpiresUtc.Value)
        {
            _memoryCache.Set(key, ticket, _memoryCacheEntryOptions);
            return;
        }
        
        var userSession = await _context.UserSessions.FirstOrDefaultAsync(x => x.Id == new Guid(key));
        var refreshToken = ticket.Properties.GetTokenValue(TokenTypes.RefreshToken);

        if (refreshToken == null)
        {
            throw new StoreException("Refresh token does not exist");
        }

        if (userSession != null)
        {
            var response = await RequestRefreshTokenAsync(refreshToken);
            
            if (response.AccessToken == null || response.RefreshToken == null || response.IdentityToken == null)
            {
                await RemoveAsync(key);
                return;
            }
            
            ticket.Properties.UpdateTokenValue(TokenTypes.AccessToken, response.AccessToken);
            ticket.Properties.UpdateTokenValue(TokenTypes.RefreshToken, response.RefreshToken);
            ticket.Properties.UpdateTokenValue(TokenTypes.IdentityToken, response.IdentityToken);
            
            var serializedTicket = _ticketSerializer.Serialize(ticket);
            
            userSession.UpdateValue(serializedTicket);
            userSession.UpdateExpiresAt(userSession.ExpiresAt.AddSeconds(response.ExpiresIn));
            
            _context.UserSessions.Update(userSession);
            await _context.SaveChangesAsync();
        }
        
        _memoryCache.Set(key, ticket, _memoryCacheEntryOptions);
    }

    public async Task<AuthenticationTicket> RetrieveAsync(string key)
    {
        var isMemoryCacheTicketExist = _memoryCache.TryGetValue(key, out AuthenticationTicket memoryCacheTicket);
        
        if (isMemoryCacheTicketExist)
        {
            return memoryCacheTicket;
        }
        
        var userSession = await _context.UserSessions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == new Guid(key));

        if (userSession == null)
        {
            return null;
        }

        var deserializedTicket = _ticketSerializer.Deserialize(userSession.Value);
        
        if (deserializedTicket == null)
        {
            throw new StoreException("Deserialization ticket error");
        }

        if (DateTimeOffset.UtcNow > userSession.ExpiresAt)
        {
            var refreshToken = deserializedTicket.Properties.GetTokenValue(TokenTypes.RefreshToken);

            if (refreshToken == null)
            {
                throw new StoreException("Refresh token does not exist");
            }
            
            var response = await RequestRefreshTokenAsync(refreshToken);

            if (response.AccessToken == null || response.RefreshToken == null || response.IdentityToken == null)
            {
                await RemoveAsync(key);
                return null;
            }
            
            deserializedTicket.Properties.UpdateTokenValue(TokenTypes.AccessToken, response.AccessToken);
            deserializedTicket.Properties.UpdateTokenValue(TokenTypes.RefreshToken, response.RefreshToken);
            deserializedTicket.Properties.UpdateTokenValue(TokenTypes.IdentityToken, response.IdentityToken);
            
            var serializedTicket = _ticketSerializer.Serialize(deserializedTicket);
        
            userSession.UpdateValue(serializedTicket);
            userSession.UpdateExpiresAt(userSession.ExpiresAt.AddSeconds(response.ExpiresIn));
            
            _context.UserSessions.Update(userSession);
            await _context.SaveChangesAsync();
        }
        
        return deserializedTicket;
    }

    public async Task RemoveAsync(string key)
    {
        var userSession = await _context.UserSessions.FirstAsync(x => x.Id == new Guid(key));
        _context.UserSessions.Remove(userSession);
        await _context.SaveChangesAsync();
    }

    private async Task<TokenResponse> RequestRefreshTokenAsync(string refreshToken)
    {
        var refreshTokenRequest = new RefreshTokenRequest
        {
            Address = _azureAdConfiguration.AzureAdTokenUrl,
            ClientId = _azureAdConfiguration.ClientId.ToString(),
            ClientSecret = _azureAdConfiguration.ClientSecret,
            GrantType = GrantType.RefreshToken,
            Scope = _azureAdConfiguration.Scopes,
            RefreshToken = refreshToken
        };
        
        var response = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

        return response;
    }
}
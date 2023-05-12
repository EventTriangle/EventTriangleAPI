using System.IdentityModel.Tokens.Jwt;
using EventTriangleAPI.Authorization.Domain.Entities;
using EventTriangleAPI.Authorization.Persistence;
using EventTriangleAPI.Shared.Application.Constants;
using EventTriangleAPI.Shared.DTO.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EventTriangleAPI.Authorization.BusinessLogic.Services;

public class TicketStore : ITicketStore
{
    private readonly DatabaseContext _context;
    private readonly TicketSerializer _ticketSerializer;
    private readonly HttpClient _httpClient;
    private readonly AzureAdConfiguration _azureAdConfiguration;
    private readonly IMemoryCache _memoryCache;

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
    }

    public async Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        var idToken = ticket.Properties.GetTokenValue("id_token");
        var handler = new JwtSecurityTokenHandler();
        var decodeToken = handler.ReadToken(idToken) as JwtSecurityToken;

        if (decodeToken == null)
        {
            throw new Exception("Read token error");
        }
        
        var sessionId = decodeToken.Claims.First(x => x.Type == "sid").Value;

        await RenewAsync(sessionId, ticket);
        return sessionId;
    }

    public async Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        var userSession = await _context.UserSessions.FirstOrDefaultAsync(x => x.Id == new Guid(key));
        
        var ticketExpiresUtc = ticket.Properties.ExpiresUtc;
        var refreshToken = ticket.Properties.GetTokenValue("refresh_token");
        
        if (ticketExpiresUtc.HasValue == false)
        {
            throw new Exception("Ticket ExpiresUtc value does not exist");
        }

        if (refreshToken == null)
        {
            throw new Exception("Refresh token does not exist");
        }

        if (DateTimeOffset.UtcNow > ticketExpiresUtc.Value && userSession != null)
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

            if (response.AccessToken == null || response.RefreshToken == null || response.IdentityToken == null)
            {
                await RemoveAsync(key);
                return;
            }
            
            ticket.Properties.UpdateTokenValue("access_token", response.AccessToken);
            ticket.Properties.UpdateTokenValue("refresh_token", response.RefreshToken);
            ticket.Properties.UpdateTokenValue("id_token", response.IdentityToken);
            
            var serializedTicket = _ticketSerializer.Serialize(ticket);
            
            userSession.UpdateValue(serializedTicket);
            userSession.UpdateExpiresAt(userSession.ExpiresAt.AddSeconds(response.ExpiresIn));
            userSession.UpdateUpdatedAt(DateTimeOffset.UtcNow);
            
            _context.UserSessions.Update(userSession);
            await _context.SaveChangesAsync();
        }

        if (userSession == null)
        {
            var serializedTicket = _ticketSerializer.Serialize(ticket);
            
            var newUserSession = new UserSessionEntity(new Guid(key), ticketExpiresUtc.Value, serializedTicket);

            _context.UserSessions.Add(newUserSession);
            await _context.SaveChangesAsync();

            var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));
            
            _memoryCache.Set(key, ticket, memoryCacheEntryOptions);
        }
    }

    public async Task<AuthenticationTicket> RetrieveAsync(string key)
    {
        var isMemoryCacheTicketExist = _memoryCache.TryGetValue(key, out AuthenticationTicket memoryCacheTicket);
        
        if (isMemoryCacheTicketExist)
        {
            return memoryCacheTicket;
        }
        
        var ticket = await _context.UserSessions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == new Guid(key));

        if (ticket == null)
        {
            return null;
        }

        if (DateTimeOffset.UtcNow > ticket.ExpiresAt)
        {
            var deserializedTicketForRenewing = _ticketSerializer.Deserialize(ticket.Value);
            await RenewAsync(key, deserializedTicketForRenewing);
            ticket = await _context.UserSessions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == new Guid(key));
        }
        
        var deserializedTicket = _ticketSerializer.Deserialize(ticket.Value);

        var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(30))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));
            
        _memoryCache.Set(key, ticket, memoryCacheEntryOptions);
        
        return deserializedTicket;
    }

    public async Task RemoveAsync(string key)
    {
        var userSession = await _context.UserSessions.FirstAsync(x => x.Id == new Guid(key));
        _context.UserSessions.Remove(userSession);
        await _context.SaveChangesAsync();
    }
}
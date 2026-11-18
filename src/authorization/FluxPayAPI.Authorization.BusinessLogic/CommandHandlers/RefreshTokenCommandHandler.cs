using System.Net;
using System.Text.Json;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.Application.Constants;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Models;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Auth;
using FluxPayAPI.Shared.DTO.Responses.Errors;

namespace FluxPayAPI.Authorization.BusinessLogic.CommandHandlers;

public class
    RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, AzureAdAuthResponse>
{
    private readonly HttpClient _httpClient;
    private readonly AzureAdConfiguration _azureAdConfiguration;

    public RefreshTokenCommandHandler(
        HttpClient httpClient,
        AzureAdConfiguration azureAdConfiguration)
    {
        _httpClient = httpClient;
        _azureAdConfiguration = azureAdConfiguration;
    }

    public async Task<IResult<AzureAdAuthResponse, Error>> HandleAsync(RefreshTokenCommand command)
    {
        var bodyDictionary = RefreshTokenBody(
            command.RefreshToken,
            _azureAdConfiguration.ClientId,
            _azureAdConfiguration.Scopes,
            _azureAdConfiguration.ClientSecret,
            _azureAdConfiguration.RedirectUri,
            GrantType.RefreshToken);

        var httpContent = new FormUrlEncodedContent(bodyDictionary);
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, _azureAdConfiguration.AzureAdTokenUrl);
        httpRequest.Content = httpContent;

        var response = await _httpClient.SendAsync(httpRequest);

        var json = await response.Content.ReadAsStringAsync();

        if (response.StatusCode != HttpStatusCode.OK)
        {
            var error = new Error(json);
            return new Result<AzureAdAuthResponse>(error);
        }

        var azAdResponse = JsonSerializer.Deserialize<AzureAdAuthResponse>(json);

        var result = new Result<AzureAdAuthResponse>(azAdResponse);

        return result;
    }

    private static Dictionary<string, string> RefreshTokenBody(
        string refreshToken,
        Guid clientId,
        string scopes,
        string clientSecret,
        string redirectUri,
        string grantType)
    {
        var dict = new Dictionary<string, string>
        {
            { "client_id", clientId.ToString() },
            { "scope", scopes },
            { "refresh_token", refreshToken },
            { "redirect_uri", redirectUri },
            { "grant_type", grantType },
            { "client_secret", clientSecret }
        };

        return dict;
    }
}
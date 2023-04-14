using System.Net;
using System.Text.Json;
using EventTriangleAPI.Authorization.BusinessLogic.Interfaces;
using EventTriangleAPI.Shared.Application.Constants;
using EventTriangleAPI.Shared.DTO.Models;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Auth;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Authorization.BusinessLogic.Handlers;

public class AzureAdService : IAzureAdService
{
    private readonly HttpClient _httpClient;
    private readonly AzureAdConfiguration _azureAdConfiguration;

    private readonly string _azureAdTokenUrl;

    public AzureAdService(
        HttpClient httpClient,
        AzureAdConfiguration azureAdConfiguration)
    {
        _httpClient = httpClient;
        _azureAdConfiguration = azureAdConfiguration;

        _azureAdTokenUrl = $"{_azureAdConfiguration.Instance}{_azureAdConfiguration.TenantId}/oauth2/v2.0/token";
    }

    public async Task<Result<AzureAdAuthResponse>> GetAccessAndIdTokensAsync(string code,
        string codeVerifier)
    {
        var bodyDictionary = AccessTokenBody(
            code, codeVerifier,
            _azureAdConfiguration.ClientId,
            _azureAdConfiguration.Scopes,
            _azureAdConfiguration.ClientSecret,
            _azureAdConfiguration.RedirectUri,
            GrantType.AuthorizationCode);

        var result = await RequestAzureAdAsync(bodyDictionary, HttpMethod.Get);

        return result;
    }

    public async Task<Result<AzureAdAuthResponse>> RefreshAccessAndIdTokensAsync(string refreshToken)
    {
        var bodyDictionary = RefreshTokenBody(
            refreshToken,
            _azureAdConfiguration.ClientId,
            _azureAdConfiguration.Scopes,
            _azureAdConfiguration.ClientSecret,
            _azureAdConfiguration.RedirectUri,
            GrantType.RefreshToken);

        var requestAzureAdAsync = await RequestAzureAdAsync(bodyDictionary, HttpMethod.Post);

        return requestAzureAdAsync;
    }

    private async Task<Result<AzureAdAuthResponse>> RequestAzureAdAsync(
        Dictionary<string, string> body,
        HttpMethod httpMethod)

    {
        var httpContent = new FormUrlEncodedContent(body);
        var httpRequest = new HttpRequestMessage(httpMethod, _azureAdTokenUrl);
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

    private static Dictionary<string, string> AccessTokenBody(
        string code,
        string codeVerifier,
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
            { "code", code },
            { "redirect_uri", redirectUri },
            { "grant_type", grantType },
            { "client_secret", clientSecret },
            { "code_verifier", codeVerifier }
        };

        return dict;
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
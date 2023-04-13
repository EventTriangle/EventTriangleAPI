using EventTriangleAPI.Authorization.BusinessLogic.Interfaces;
using EventTriangleAPI.Shared.DTO.Models;
using EventTriangleAPI.Shared.DTO.Responses;
using Newtonsoft.Json;

namespace EventTriangleAPI.Authorization.BusinessLogic.Services;

public class AzureAdService : IAzureAdService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerSettings _jsonSerializerSettings;
    private readonly AzureAdConfiguration _azureAdConfiguration;

    private readonly string _azureAdTokenUrl;

    public AzureAdService(
        HttpClient httpClient,
        JsonSerializerSettings jsonSerializerSettings, 
        AzureAdConfiguration azureAdConfiguration)
    {
        _httpClient = httpClient;
        _jsonSerializerSettings = jsonSerializerSettings;
        _azureAdConfiguration = azureAdConfiguration;

        _azureAdTokenUrl = $"{_azureAdConfiguration.Instance}{_azureAdConfiguration.TenantId}/oauth2/v2.0/token";
    }

    public async Task<AzureAdAuthorizationDataResponse> GetAccessAndIdTokensAsync(string code, string codeVerifier)
    {
        var clientSecret = Environment.GetEnvironmentVariable("azure_ad_client_secret");

        if (string.IsNullOrEmpty(clientSecret))
        {
            throw new ArgumentNullException(nameof(clientSecret));
        }

        var requestDictionary = AzureAdRequestDictionary(
            code,
            codeVerifier,
            _azureAdConfiguration.ClientId,
            _azureAdConfiguration.Scopes,
            clientSecret);

        var httpContent = new FormUrlEncodedContent(requestDictionary);
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, _azureAdTokenUrl);
        httpRequest.Content = httpContent;

        var response = await _httpClient.SendAsync(httpRequest);

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<AzureAdAuthorizationDataResponse>(json, _jsonSerializerSettings);

        return result;
    }

    public async Task<AzureAdAuthorizationDataResponse> RefreshAccessAndIdTokensAsync(string code, string codeVerifier)
    {
        var clientSecret = Environment.GetEnvironmentVariable("azure_ad_client_secret");
        
        if (string.IsNullOrEmpty(clientSecret))
        {
            throw new ArgumentNullException(nameof(clientSecret));
        }

        var requestDictionary = AzureAdRequestDictionary(
            code,
            codeVerifier,
            _azureAdConfiguration.ClientId,
            _azureAdConfiguration.Scopes,
            clientSecret);

        var httpContent = new FormUrlEncodedContent(requestDictionary);
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, _azureAdTokenUrl);
        httpRequest.Content = httpContent;

        var response = await _httpClient.SendAsync(httpRequest);

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<AzureAdAuthorizationDataResponse>(json, _jsonSerializerSettings);

        return result;
    }

    private static Dictionary<string, string> AzureAdRequestDictionary(
        string code,
        string codeVerifier,
        Guid clientId,
        string scopes,
        string clientSecret)
    {
        var dict = new Dictionary<string, string>
        {
            { "client_id", clientId.ToString() },
            { "scope", $"api://{clientId}/{scopes} offline_access openid" },
            { "code", code },
            { "redirect_uri", "http://localhost:3000" },
            { "grant_type", "authorization_code" },
            { "client_secret", clientSecret },
            { "code_verifier", codeVerifier }
        };

        return dict;
    }
}
using EventTriangleAPI.Authorization.BusinessLogic.Interfaces;
using EventTriangleAPI.Shared.Application.Services;
using EventTriangleAPI.Shared.DTO.Responses;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EventTriangleAPI.Authorization.BusinessLogic.Services;

public class AzureAdService : IAzureAdService
{
    private readonly HttpClient _httpClient;
    private readonly AppSettingsService _appSettingsService;

    public AzureAdService(HttpClient httpClient, AppSettingsService appSettingsService)
    {
        _httpClient = httpClient;
        _appSettingsService = appSettingsService;
    }

    public async Task<AzureAdTokenResponse> GetAuthorizationData(string code, string codeVerifier)
    {
        var appSettingsPath = _appSettingsService.GetAppSettingsPathSender();

        var configuration = new ConfigurationBuilder()
            .AddJsonFile(appSettingsPath)
            .Build();

        var instance = configuration["AzureAd:Instance"];
        var tenantId = configuration["AzureAd:TenantId"];
        var clientId = configuration["AzureAd:ClientId"];
        var scopes = configuration["AzureAd:Scopes"];
        var clientSecret = Environment.GetEnvironmentVariable("azure_ad_client_secret") ?? string.Empty;
        
        var url = $"{instance}{tenantId}/oauth2/v2.0/token";

        var dict = new Dictionary<string, string>
        {
            { "client_id", clientId },
            { "scope", $"api://{clientId}/{scopes} offline_access" },
            { "code", code },
            { "redirect_uri", "http://localhost:3000" },
            { "grant_type", "authorization_code" },
            { "client_secret", clientSecret },
            { "code_verifier", codeVerifier },
        };
        
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, url) { Content = new FormUrlEncodedContent(dict) };
        
        var response = await _httpClient.SendAsync(httpRequest);
        
        var json = await response.Content.ReadAsStringAsync();

        var jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };
        
        var result = JsonConvert.DeserializeObject<AzureAdTokenResponse>(json, jsonSerializerSettings);
        
        return result;
    }
}
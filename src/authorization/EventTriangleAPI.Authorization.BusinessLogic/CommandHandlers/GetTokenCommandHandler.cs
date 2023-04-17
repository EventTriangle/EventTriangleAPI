using System.Net;
using System.Text.Json;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.Application.Constants;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Commands.Auth;
using EventTriangleAPI.Shared.DTO.Models;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Auth;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Authorization.BusinessLogic.CommandHandlers;

public class GetTokenCommandHandler : ICommandHandler<GetTokenBody, AzureAdAuthResponse>
{
    private readonly HttpClient _httpClient;
    private readonly AzureAdConfiguration _azureAdConfiguration;

    public GetTokenCommandHandler(
        HttpClient httpClient,
        AzureAdConfiguration azureAdConfiguration)
    {
        _httpClient = httpClient;
        _azureAdConfiguration = azureAdConfiguration;
    }

    public async Task<IResult<AzureAdAuthResponse, Error>> HandleAsync(ICommand<GetTokenBody> command)
    {
        var bodyDictionary = AccessTokenBody(
            command.Body.Code,
            command.Body.CodeVerifier,
            _azureAdConfiguration.ClientId,
            _azureAdConfiguration.Scopes,
            _azureAdConfiguration.ClientSecret,
            _azureAdConfiguration.RedirectUri,
            GrantType.AuthorizationCode);

        var httpContent = new FormUrlEncodedContent(bodyDictionary);
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, _azureAdConfiguration.AzureAdTokenUrl);
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
}
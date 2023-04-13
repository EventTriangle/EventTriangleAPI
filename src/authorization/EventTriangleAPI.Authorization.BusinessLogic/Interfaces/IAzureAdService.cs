using EventTriangleAPI.Shared.DTO.Responses;

namespace EventTriangleAPI.Authorization.BusinessLogic.Interfaces;

public interface IAzureAdService
{
    Task<AzureAdAuthorizationDataResponse> GetAccessAndIdTokensAsync(string code, string codeVerifier);

    Task<AzureAdAuthorizationDataResponse> RefreshAccessAndIdTokensAsync(string code, string codeVerifier);
} 
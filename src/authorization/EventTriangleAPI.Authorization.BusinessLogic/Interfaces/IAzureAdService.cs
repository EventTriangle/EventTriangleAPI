using EventTriangleAPI.Shared.DTO.Responses;

namespace EventTriangleAPI.Authorization.BusinessLogic.Interfaces;

public interface IAzureAdService
{
    Task<Result<AzureAdAuthorizationDataResponse>> GetAccessAndIdTokensAsync(string code, string codeVerifier);

    Task<Result<AzureAdAuthorizationDataResponse>> RefreshAccessAndIdTokensAsync(string refreshToken);
} 
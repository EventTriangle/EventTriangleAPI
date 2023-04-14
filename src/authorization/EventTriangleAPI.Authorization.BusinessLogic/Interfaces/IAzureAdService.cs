using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Auth;

namespace EventTriangleAPI.Authorization.BusinessLogic.Interfaces;

public interface IAzureAdService
{
    Task<Result<AzureAdAuthResponse>> GetAccessAndIdTokensAsync(string code, string codeVerifier);

    Task<Result<AzureAdAuthResponse>> RefreshAccessAndIdTokensAsync(string refreshToken);
} 
using EventTriangleAPI.Shared.DTO.Responses;

namespace EventTriangleAPI.Authorization.BusinessLogic.Interfaces;

public interface IAzureAdService
{
    Task<AzureAdTokenResponse> GetAuthorizationData(string code, string codeVerifier);
} 
using EventTriangleAPI.Shared.DTO.Responses;

namespace EventTriangleAPI.Authorization.BusinessLogic.Interfaces;

public interface IAzureAdService
{
    Task<AzureAdAuthorizationDataResponse> GetAuthorizationData(string code, string codeVerifier);

    Task<AzureAdAuthorizationDataResponse> GetRefreshData(string code, string codeVerifier);
} 
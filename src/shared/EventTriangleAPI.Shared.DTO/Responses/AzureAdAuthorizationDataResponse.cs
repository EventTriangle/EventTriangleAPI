namespace EventTriangleAPI.Shared.DTO.Responses;

public record AzureAdAuthorizationDataResponse(
    string TokenType,
    string Scope,
    int ExpiresIn,
    int ExtExpiresIn,
    string AccessToken,
    string RefreshToken,
    string IdToken
);
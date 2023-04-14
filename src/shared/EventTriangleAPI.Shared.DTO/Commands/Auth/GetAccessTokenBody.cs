namespace EventTriangleAPI.Shared.DTO.Commands.Auth;

public record GetAccessTokenBody(string Code, string CodeVerifier);
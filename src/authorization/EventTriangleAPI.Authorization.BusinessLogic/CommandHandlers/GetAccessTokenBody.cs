namespace EventTriangleAPI.Shared.DTO.Commands.Auth;

public record GetTokenBody(string Code, string CodeVerifier);
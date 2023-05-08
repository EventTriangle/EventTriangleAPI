namespace EventTriangleAPI.Authorization.BusinessLogic.CommandHandlers;

public record GetTokenBody(string Code, string CodeVerifier);
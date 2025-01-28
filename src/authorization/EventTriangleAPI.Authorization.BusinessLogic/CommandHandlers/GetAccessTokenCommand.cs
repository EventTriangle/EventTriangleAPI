using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Authorization.BusinessLogic.CommandHandlers;

public record GetTokenCommand(string Code, string CodeVerifier) : ICommand;
using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Authorization.BusinessLogic.CommandHandlers;

public record GetTokenCommand(string Code, string CodeVerifier) : ICommand;
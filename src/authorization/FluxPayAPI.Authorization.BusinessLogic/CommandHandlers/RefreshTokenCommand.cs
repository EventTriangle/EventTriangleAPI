using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Authorization.BusinessLogic.CommandHandlers;

public record RefreshTokenCommand(string RefreshToken) : ICommand;
using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Authorization.BusinessLogic.CommandHandlers;

public record RefreshTokenCommand(string RefreshToken) : ICommand;
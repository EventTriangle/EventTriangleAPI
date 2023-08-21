using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public record SuspendUserCommand(string RequesterId, string UserId) : ICommand;
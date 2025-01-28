using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record SuspendUserCommand(string RequesterId, string UserId) : ICommand;
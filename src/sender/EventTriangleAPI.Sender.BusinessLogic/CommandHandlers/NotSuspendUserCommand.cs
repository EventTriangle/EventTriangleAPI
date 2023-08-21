using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record NotSuspendUserCommand(string RequesterId, string UserId) : ICommand;
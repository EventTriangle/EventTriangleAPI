using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public record NotSuspendUserCommand(string RequesterId, string UserId) : ICommand;
using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public record SuspendUserCommand(string RequesterId, string UserId) : ICommand;
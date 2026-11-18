using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public record NotSuspendUserCommand(string RequesterId, string UserId) : ICommand;
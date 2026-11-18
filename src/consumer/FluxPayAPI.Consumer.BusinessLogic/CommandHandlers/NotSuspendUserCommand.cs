using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;

public record NotSuspendUserCommand(string RequesterId, string UserId) : ICommand;
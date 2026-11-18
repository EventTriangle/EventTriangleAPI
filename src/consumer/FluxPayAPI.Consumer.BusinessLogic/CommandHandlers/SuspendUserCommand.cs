using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;

public record SuspendUserCommand(string RequesterId, string UserId) : ICommand;
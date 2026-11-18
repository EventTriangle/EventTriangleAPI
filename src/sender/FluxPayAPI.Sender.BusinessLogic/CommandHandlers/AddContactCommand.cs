using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public record AddContactCommand(string RequesterId, string ContactId) : ICommand;
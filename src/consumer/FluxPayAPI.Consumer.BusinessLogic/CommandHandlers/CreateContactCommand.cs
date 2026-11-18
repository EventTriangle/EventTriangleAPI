using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;

public record CreateContactCommand(string RequesterId, string ContactId) : ICommand;
using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;

public record DeleteContactCommand(string RequesterId, string ContactId) : ICommand;
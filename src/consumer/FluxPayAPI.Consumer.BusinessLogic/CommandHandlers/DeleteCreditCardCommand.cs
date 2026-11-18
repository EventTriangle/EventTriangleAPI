using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;

public record DeleteCreditCardCommand(string RequesterId, Guid CardId) : ICommand;
using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public record DeleteCreditCardCommand(string RequesterId, Guid CardId) : ICommand;
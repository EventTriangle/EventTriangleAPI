using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;

public record CreateTransactionCardToUserCommand(
    Guid CreditCardId, 
    string RequesterId,
    decimal Amount,
    DateTime CreatedAt) : ICommand;
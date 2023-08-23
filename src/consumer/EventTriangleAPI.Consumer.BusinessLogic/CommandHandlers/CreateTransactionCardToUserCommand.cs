using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public record CreateTransactionCardToUserCommand(
    Guid CreditCardId, 
    string RequesterId,
    decimal Amount,
    DateTime CreatedAt) : ICommand;
using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public record CreateTransactionCardToUserCommand(
    Guid CreditCardId, 
    string ToUserId,
    decimal Amount) : ICommand;
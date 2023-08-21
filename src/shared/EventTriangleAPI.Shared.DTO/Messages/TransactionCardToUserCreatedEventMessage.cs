namespace EventTriangleAPI.Shared.DTO.Messages;

public record TransactionCardToUserCreatedEventMessage(
    Guid Id, 
    string RequesterId,
    Guid CreditCardId, 
    decimal Amount,
    DateTime CreatedAt);
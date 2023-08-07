namespace EventTriangleAPI.Shared.DTO.Messages;

public record TransactionCardToUserCreatedEventMessage(
    Guid Id, 
    Guid CreditCardId, 
    string ToUserId,
    decimal Amount,
    DateTime CreatedAt);
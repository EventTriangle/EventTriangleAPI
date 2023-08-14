namespace EventTriangleAPI.Shared.DTO.Messages;

public record TransactionRollBackedEventMessage(
    Guid Id, 
    string RequesterId, 
    Guid TransactionId,
    DateTime CreatedAt);
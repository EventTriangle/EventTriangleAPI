namespace EventTriangleAPI.Shared.DTO.Messages;

public record TransactionRollBackedEventMessage(Guid Id, Guid TransactionId, DateTime CreatedAt);
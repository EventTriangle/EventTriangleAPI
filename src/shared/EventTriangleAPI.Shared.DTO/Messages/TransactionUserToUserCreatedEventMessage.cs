namespace EventTriangleAPI.Shared.DTO.Messages;

public record TransactionUserToUserCreatedEventMessage(
    Guid Id, 
    string RequesterId, 
    string ToUserId,
    decimal Amount,
    DateTime CreatedAt);
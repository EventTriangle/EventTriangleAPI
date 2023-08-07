namespace EventTriangleAPI.Shared.DTO.Messages;

public record TransactionUserToUserCreatedEventMessage(
    Guid Id, 
    string FromUserId, 
    string ToUserId,
    decimal Amount,
    DateTime CreatedAt);
namespace EventTriangleAPI.Shared.DTO.Messages;

public record CreditCardDeletedEventMessage(Guid Id, string UserId, Guid CardId, DateTime CreatedAt);
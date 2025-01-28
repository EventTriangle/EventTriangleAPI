namespace EventTriangleAPI.Shared.DTO.Messages;

public record CreditCardDeletedEventMessage(Guid Id, string RequesterId, Guid CardId, DateTime CreatedAt);
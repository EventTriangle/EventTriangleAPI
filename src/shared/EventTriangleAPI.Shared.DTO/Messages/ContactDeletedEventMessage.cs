namespace EventTriangleAPI.Shared.DTO.Messages;

public record ContactDeletedEventMessage(Guid Id, string RequesterId, string ContactId, DateTime CreatedAt);
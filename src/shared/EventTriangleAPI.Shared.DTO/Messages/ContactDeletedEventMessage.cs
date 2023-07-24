namespace EventTriangleAPI.Shared.DTO.Messages;

public record ContactDeletedEventMessage(Guid Id, string UserId, string ContactId, DateTime CreatedAt);
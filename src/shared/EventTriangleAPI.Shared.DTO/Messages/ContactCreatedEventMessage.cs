namespace EventTriangleAPI.Shared.DTO.Messages;

public record ContactCreatedEventMessage(Guid Id, string UserId, string ContactId, DateTime CreatedAt);
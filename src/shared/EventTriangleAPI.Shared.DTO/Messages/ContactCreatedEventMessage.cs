namespace EventTriangleAPI.Shared.DTO.Messages;

public record ContactCreatedEventMessage(Guid Id, string RequesterId, string ContactId, DateTime CreatedAt);
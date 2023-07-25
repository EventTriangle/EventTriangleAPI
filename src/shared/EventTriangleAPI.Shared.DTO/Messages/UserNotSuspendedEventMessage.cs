namespace EventTriangleAPI.Shared.DTO.Messages;

public record UserNotSuspendedEventMessage(Guid Id, string UserId, DateTime CreatedAt);
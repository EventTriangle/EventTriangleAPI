namespace EventTriangleAPI.Shared.DTO.Messages;

public record UserNotSuspendedEventMessage(
    Guid Id,
    string RequesterId,
    string UserId,
    DateTime CreatedAt);
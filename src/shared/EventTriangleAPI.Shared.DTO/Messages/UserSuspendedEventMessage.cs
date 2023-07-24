namespace EventTriangleAPI.Shared.DTO.Messages;

public record UserSuspendedEventMessage(Guid Id, string UserId, DateTime CreatedAt);
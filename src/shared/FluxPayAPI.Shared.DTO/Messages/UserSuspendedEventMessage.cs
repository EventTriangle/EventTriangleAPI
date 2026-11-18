namespace FluxPayAPI.Shared.DTO.Messages;

public record UserSuspendedEventMessage(
    Guid Id,
    string RequesterId,
    string UserId, 
    DateTime CreatedAt);
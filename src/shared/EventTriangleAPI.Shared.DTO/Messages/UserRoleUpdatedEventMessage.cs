using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Shared.DTO.Messages;

public record UserRoleUpdatedEventMessage(
    Guid Id,
    string RequesterId,
    string UserId, 
    UserRole UserRole,
    DateTime CreatedAt);
using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Shared.DTO.Messages;

public record UserRoleUpdatedEventMessage(
    Guid Id,
    string RequesterId,
    string UserId, 
    UserRole UserRole,
    DateTime CreatedAt);
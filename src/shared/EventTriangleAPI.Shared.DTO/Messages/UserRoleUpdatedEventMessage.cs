using EventTriangleAPI.Shared.Application.Enums;

namespace EventTriangleAPI.Shared.DTO.Messages;

public record UserRoleUpdatedEventMessage(Guid Id, string UserId, UserRole UserRole, DateTime CreatedAt);
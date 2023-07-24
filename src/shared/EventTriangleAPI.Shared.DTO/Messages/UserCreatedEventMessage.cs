using EventTriangleAPI.Authorization.Domain.Enums;
using EventTriangleAPI.Shared.Application.Enums;

namespace EventTriangleAPI.Shared.DTO.Messages;

public record UserCreatedEventMessage(Guid Id, string UserId, UserRole UserRole, UserStatus UserStatus, DateTime CreatedAt);
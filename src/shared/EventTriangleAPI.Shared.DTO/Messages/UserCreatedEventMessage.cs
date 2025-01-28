using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Shared.DTO.Messages;

public record UserCreatedEventMessage(
    Guid Id, 
    string UserId, 
    string Email, 
    UserRole UserRole, 
    UserStatus UserStatus, 
    DateTime CreatedAt);
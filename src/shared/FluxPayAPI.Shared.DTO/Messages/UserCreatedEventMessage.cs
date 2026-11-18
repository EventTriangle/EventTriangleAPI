using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Shared.DTO.Messages;

public record UserCreatedEventMessage(
    Guid Id, 
    string UserId, 
    string Email, 
    UserRole UserRole, 
    UserStatus UserStatus, 
    DateTime CreatedAt);
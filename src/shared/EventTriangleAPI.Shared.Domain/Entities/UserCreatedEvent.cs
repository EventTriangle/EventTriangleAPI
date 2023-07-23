using EventTriangleAPI.Shared.Domain.Entities.Validation;
using EventTriangleAPI.Shared.Domain.Enums;
using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Entities;

public class UserCreatedEvent
{
    public Guid Id { get; private set; }
    
    public string UserId { get; private set; }
    
    public UserRole UserRole { get; private set; }
    
    public UserStatus UserStatus { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public UserCreatedEvent(string userId, UserRole userRole, UserStatus userStatus)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        UserRole = userRole;
        UserStatus = userStatus;
        CreatedAt = DateTime.UtcNow;
        
        new UserCreatedEventValidator().ValidateAndThrow(this);
    }
}
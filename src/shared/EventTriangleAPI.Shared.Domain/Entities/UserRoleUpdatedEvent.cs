using EventTriangleAPI.Shared.Domain.Entities.Validation;
using EventTriangleAPI.Shared.Domain.Enums;
using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Entities;

public class UserRoleUpdatedEvent
{
    public Guid Id { get; private set; }
    
    public string UserId { get; private set; }
    
    public UserRole UserRole { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public UserRoleUpdatedEvent(string userId, UserRole userRole)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        UserRole = userRole;
        CreatedAt = DateTime.UtcNow;
        
        new UserRoleUpdatedEventValidator().ValidateAndThrow(this);
    }
}
using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.Application.Enums;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class UserRoleUpdatedEvent
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public UserRole UserRole { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public UserRoleUpdatedEvent(Guid userId, UserRole userRole)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        UserRole = userRole;
        CreatedAt = DateTime.UtcNow;
        
        new UserRoleUpdatedEventValidator().ValidateAndThrow(this);
    }
}
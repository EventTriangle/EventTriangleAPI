using EventTriangleAPI.Sender.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class UserSuspendedEvent
{
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public UserSuspendedEvent(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        
        new UserSuspendedEventValidator().ValidateAndThrow(this);
    }
}
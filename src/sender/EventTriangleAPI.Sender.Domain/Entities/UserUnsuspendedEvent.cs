using EventTriangleAPI.Sender.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class UserUnsuspendedEvent
{
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public UserUnsuspendedEvent(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        
        new UserUnsuspendedEventValidator().ValidateAndThrow(this);
    }
}
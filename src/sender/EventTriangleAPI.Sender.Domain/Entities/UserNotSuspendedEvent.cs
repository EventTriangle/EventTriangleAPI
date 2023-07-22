using EventTriangleAPI.Sender.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class UserNotSuspendedEvent
{
    public Guid Id { get; private set; }
    
    public string UserId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public UserNotSuspendedEvent(string userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        
        new UserNotSuspendedEventValidator().ValidateAndThrow(this);
    }
}
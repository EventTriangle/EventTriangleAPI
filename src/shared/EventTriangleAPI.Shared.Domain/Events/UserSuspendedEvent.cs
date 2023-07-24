using EventTriangleAPI.Shared.Domain.Events.Validation;
using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Events;

public class UserSuspendedEvent
{
    public Guid Id { get; private set; }
    
    public string UserId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public UserSuspendedEvent(string userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        
        new UserSuspendedEventValidator().ValidateAndThrow(this);
    }
}
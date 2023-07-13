using EventTriangleAPI.Sender.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class UserCreatedEvent
{
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public UserCreatedEvent(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        
        new UserCreatedEventValidator().ValidateAndThrow(this);
    }
}
using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

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

    public UserSuspendedEventMessage CreateEventMessage()
    {
        return new UserSuspendedEventMessage(Id, UserId, CreatedAt);
    }
}
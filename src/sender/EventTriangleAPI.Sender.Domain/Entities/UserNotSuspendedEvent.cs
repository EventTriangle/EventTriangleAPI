using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class UserNotSuspendedEvent
{
    public Guid Id { get; private set; }
    
    public string RequesterId { get; set; }
    
    public string UserId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public UserNotSuspendedEvent(string requesterId, string userId)
    {
        Id = Guid.NewGuid();
        RequesterId = requesterId;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        
        new UserNotSuspendedEventValidator().ValidateAndThrow(this);
    }

    public UserNotSuspendedEventMessage CreateEventMessage()
    {
        return new UserNotSuspendedEventMessage(Id, RequesterId, UserId, CreatedAt);
    }
}
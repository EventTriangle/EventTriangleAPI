using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class UserSuspendedEvent
{
    public Guid Id { get; private set; }
    
    public string RequesterId { get; private set; }
    
    public string UserId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    private static readonly UserSuspendedEventValidator Validator = new(); 

    public UserSuspendedEvent(string requesterId, string userId)
    {
        Id = Guid.NewGuid();
        RequesterId = requesterId;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        
        Validator.ValidateAndThrow(this);
    }

    public UserSuspendedEventMessage CreateEventMessage()
    {
        return new UserSuspendedEventMessage(Id, RequesterId, UserId, CreatedAt);
    }
}
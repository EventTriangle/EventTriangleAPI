using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class UserRoleUpdatedEvent
{
    public Guid Id { get; private set; }
    
    public string RequesterId { get; private set; }
    
    public string UserId { get; private set; }
    
    public UserRole UserRole { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    private static readonly UserRoleUpdatedEventValidator Validator = new(); 

    public UserRoleUpdatedEvent(string requesterId, string userId, UserRole userRole)
    {
        Id = Guid.NewGuid();
        RequesterId = requesterId;
        UserId = userId;
        UserRole = userRole;
        CreatedAt = DateTime.UtcNow;
        
        Validator.ValidateAndThrow(this);
    }

    public UserRoleUpdatedEventMessage CreateEventMessage()
    {
        return new UserRoleUpdatedEventMessage(Id, RequesterId, UserId, UserRole, CreatedAt);
    }
}
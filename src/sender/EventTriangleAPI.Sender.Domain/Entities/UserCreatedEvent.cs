using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class UserCreatedEvent
{
    public Guid Id { get; private set; }
    
    public string UserId { get; private set; }
    
    public string Email { get; private set; }
    
    public UserRole UserRole { get; private set; }
    
    public UserStatus UserStatus { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    private static readonly UserCreatedEventValidator Validator = new(); 

    public UserCreatedEvent(string userId, string email, UserRole userRole, UserStatus userStatus)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Email = email;
        UserRole = userRole;
        UserStatus = userStatus;
        CreatedAt = DateTime.UtcNow;
        
        Validator.ValidateAndThrow(this);
    }

    public UserCreatedEventMessage CreateEventMessage()
    {
        return new UserCreatedEventMessage(Id, UserId, Email, UserRole, UserStatus, CreatedAt);
    }
}
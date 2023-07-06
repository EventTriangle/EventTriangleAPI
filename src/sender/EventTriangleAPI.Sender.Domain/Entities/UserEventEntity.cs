using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.Application.Enums.Events;
using FluentValidation;
using Uuids;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class UserEventEntity
{
    public Guid Id { get; private set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();
    
    public Guid UserId { get; private set; }
    
    public UserEventType UserEventType { get; private set; }
    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public UserEventEntity(Guid userId, UserEventType userEventType)
    {
        UserId = userId;
        UserEventType = userEventType;
        
        new UserEventEntityValidator().ValidateAndThrow(this);
    }
}
using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.Application.Enums.Events;
using FluentValidation;
using Uuids;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class ContactEventEntity
{
    public Guid Id { get; private set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();

    public Guid UserId { get; private set; }
    
    public Guid ContactId { get; private set; }
    
    public ContactEventType ContactEventType { get; private set; }

    public ContactEventEntity(Guid userId, Guid contactId, ContactEventType contactEventType)
    {
        UserId = userId;
        ContactId = contactId;
        ContactEventType = contactEventType;
        
        new ContactEventEntityValidator().ValidateAndThrow(this);
    }
}
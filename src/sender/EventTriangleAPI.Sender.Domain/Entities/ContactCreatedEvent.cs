using EventTriangleAPI.Sender.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class ContactCreatedEvent
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Guid ContactId { get; private set; }

    public DateTime CreatedAt { get; private set; }
    
    public ContactCreatedEvent(Guid userId, Guid contactId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ContactId = contactId;
        CreatedAt = DateTime.UtcNow;
        
        new ContactCreatedEventValidator().ValidateAndThrow(this);
    }
}
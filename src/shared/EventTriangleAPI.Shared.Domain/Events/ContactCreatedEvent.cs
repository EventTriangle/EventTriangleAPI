using EventTriangleAPI.Shared.Domain.Events.Validation;
using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Events;

public class ContactCreatedEvent
{
    public Guid Id { get; private set; }

    public string UserId { get; private set; }

    public string ContactId { get; private set; }

    public DateTime CreatedAt { get; private set; }
    
    public ContactCreatedEvent(string userId, string contactId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ContactId = contactId;
        CreatedAt = DateTime.UtcNow;
        
        new ContactCreatedEventValidator().ValidateAndThrow(this);
    }
}
using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class ContactCreatedEvent
{
    public Guid Id { get; private set; }

    public string RequesterId { get; private set; }

    public string ContactId { get; private set; }

    public DateTime CreatedAt { get; private set; }
    
    public ContactCreatedEvent(string requesterId, string contactId)
    {
        Id = Guid.NewGuid();
        RequesterId = requesterId;
        ContactId = contactId;
        CreatedAt = DateTime.UtcNow;
        
        new ContactCreatedEventValidator().ValidateAndThrow(this);
    }

    public ContactCreatedEventMessage CreateEventMessage()
    {
        return new ContactCreatedEventMessage(Id, RequesterId, ContactId, CreatedAt);
    }
}
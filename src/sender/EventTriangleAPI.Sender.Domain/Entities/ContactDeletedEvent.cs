using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class ContactDeletedEvent
{
    public Guid Id { get; private set; }

    public string RequesterId { get; private set; }

    public string ContactId { get; private set; }

    public DateTime CreatedAt { get; private set; }
    
    public ContactDeletedEvent(string requesterId, string contactId)
    {
        Id = Guid.NewGuid();
        RequesterId = requesterId;
        ContactId = contactId;
        CreatedAt = DateTime.UtcNow;
        
        new ContactDeletedEventValidator().ValidateAndThrow(this);
    }

    public ContactDeletedEventMessage CreateEventMessage()
    {
        return new ContactDeletedEventMessage(Id, RequesterId, ContactId, CreatedAt);
    }
}
using EventTriangleAPI.Shared.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Entities;

public class ContactDeletedEvent
{
    public Guid Id { get; private set; }

    public string UserId { get; private set; }

    public string ContactId { get; private set; }

    public DateTime CreatedAt { get; private set; }
    
    public ContactDeletedEvent(string userId, string contactId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ContactId = contactId;
        CreatedAt = DateTime.UtcNow;
        
        new ContactDeletedEventValidator().ValidateAndThrow(this);
    }
}
using EventTriangleAPI.Consumer.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class ContactEntity
{
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public Guid ContactId { get; private set; }
    
    public string ContactUsername { get; private set; }

    public ContactEntity(Guid userId, Guid contactId, string contactUsername)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ContactId = contactId;
        ContactUsername = contactUsername;

        new ContactEntityValidator().ValidateAndThrow(this);
    }
}
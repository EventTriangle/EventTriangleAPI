using EventTriangleAPI.Consumer.Domain.Entities.Validation;
using FluentValidation;
using Uuids;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class ContactEntity
{
    public Guid Id { get; private set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();
    
    public Guid UserId { get; private set; }
    
    public Guid ContactId { get; private set; }
    
    public string ContactUsername { get; private set; }

    public ContactEntity(Guid userId, Guid contactId, string contactUsername)
    {
        UserId = userId;
        ContactId = contactId;
        ContactUsername = contactUsername;

        new ContactEntityValidator().ValidateAndThrow(this);
    }
}
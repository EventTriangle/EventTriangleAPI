using EventTriangleAPI.Consumer.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class ContactEntity
{
    public string UserId { get; private set; }
    
    public UserEntity User { get; private set; }
    
    public string ContactId { get; private set; }

    public UserEntity Contact { get; private set; }
    
    public string ContactUsername { get; private set; }

    public ContactEntity(string userId, string contactId, string contactUsername)
    {
        UserId = userId;
        ContactId = contactId;
        ContactUsername = contactUsername;

        new ContactEntityValidator().ValidateAndThrow(this);
    }
}
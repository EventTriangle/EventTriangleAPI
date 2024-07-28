using EventTriangleAPI.Consumer.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class ContactEntity
{
    public string UserId { get; private set; }
    
    public UserEntity User { get; private set; }
    
    public string ContactId { get; private set; }

    public UserEntity Contact { get; private set; }

    private static readonly ContactEntityValidator Validator = new(); 
    
    public ContactEntity(string userId, string contactId)
    {
        UserId = userId;
        ContactId = contactId;

        Validator.ValidateAndThrow(this);
    }
}
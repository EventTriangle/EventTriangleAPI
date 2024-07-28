using EventTriangleAPI.Authorization.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Authorization.Domain.Entities;

public class UserEntity
{
    public string Id { get; private set; }
    
    public string Email { get; private set; }
    
    public List<UserSessionEntity> UserSessionEntities { get; private set; } = new();
    
    private static readonly UserEntityValidator Validator = new(); 

    public UserEntity(string id, string email)
    {
        Id = id;
        Email = email;
        
        Validator.ValidateAndThrow(this);
    }
}
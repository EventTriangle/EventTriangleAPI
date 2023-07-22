using EventTriangleAPI.Authorization.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Authorization.Domain.Entities;

public class UserEntity
{
    public string Id { get; private set; }
    
    public string Username { get; private set; }
    
    public List<UserSessionEntity> UserSessionEntities { get; private set; } = new();

    public UserEntity(string id, string username)
    {
        Id = id;
        Username = username;
        
        new UserEntityValidator().ValidateAndThrow(this);
    }
}
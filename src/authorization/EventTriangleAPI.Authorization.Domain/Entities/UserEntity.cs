using EventTriangleAPI.Authorization.Domain.Entities.Validation;
using EventTriangleAPI.Authorization.Domain.Enums;
using FluentValidation;
using Uuids;

namespace EventTriangleAPI.Authorization.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; private set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();
    
    public string Sub { get; private set; }
    
    public string Username { get; private set; }
    
    public UserRole Role { get; private set; }
    
    public UserStatus Status { get; private set; }

    public List<UserSessionEntity> UserSessionEntities { get; private set; } = new();

    public UserEntity(string sub, string username, UserRole role, UserStatus status)
    {
        Sub = sub;
        Username = username;
        Role = role;
        Status = status;
        
        new UserEntityValidator().ValidateAndThrow(this);
    }
}
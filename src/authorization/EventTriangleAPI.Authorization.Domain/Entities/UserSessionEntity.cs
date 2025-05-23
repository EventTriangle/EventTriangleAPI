using EventTriangleAPI.Authorization.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Authorization.Domain.Entities;

public class UserSessionEntity
{
    public Guid Id { get; private set; }
    
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    
    public DateTimeOffset ExpiresAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset DateOfLastAccess { get; private set; } = DateTimeOffset.UtcNow;
    
    public byte[] Value { get; private set; }
    
    public string UserId { get; private set; }
    
    public UserEntity User { get; private set; }

    private static readonly UserSessionEntityValidator Validator = new(); 
    
    public UserSessionEntity(Guid id, DateTimeOffset expiresAt, byte[] value, string userId)
    {
        Id = id;
        ExpiresAt = expiresAt;
        Value = value;
        UserId = userId;
        
        Validator.ValidateAndThrow(this);
    }

    public void UpdateValue(byte[] value)
    {
        Value = value;
        UpdatedAt = DateTimeOffset.UtcNow;
        
        Validator.ValidateAndThrow(this);
    }
    
    public void UpdateExpiresAt(DateTimeOffset expiresAt)
    {
        ExpiresAt = expiresAt;
        UpdatedAt = DateTimeOffset.UtcNow;
        
        Validator.ValidateAndThrow(this);
    }

    public void UpdateDateOfLastAccess()
    {
        DateOfLastAccess = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        
        Validator.ValidateAndThrow(this);
    }
}
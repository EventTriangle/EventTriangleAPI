namespace EventTriangleAPI.Authorization.Domain.Entities;

public class UserSessionEntity
{
    public Guid Id { get; private set; }
    
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    
    public DateTimeOffset ExpiresAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;
    
    public byte[] Value { get; private set; }

    public UserSessionEntity(Guid id, DateTimeOffset expiresAt, byte[] value)
    {
        Id = id;
        ExpiresAt = expiresAt;
        Value = value;
    }

    public void UpdateValue(byte[] value)
    {
        Value = value;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    
    public void UpdateExpiresAt(DateTimeOffset expiresAt)
    {
        ExpiresAt = expiresAt;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
using EventTriangleAPI.Authorization.Domain.Enums;
using Uuids;

namespace EventTriangleAPI.Authorization.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();
    
    public string Sub { get; set; }
    
    public string Username { get; set; }
    
    public decimal Balance { get; set; }
    
    public UserRole Role { get; set; }
    
    public UserStatus Status { get; set; }

    public List<UserSessionEntity> UserSessionEntities { get; set; } = new();

    public UserEntity(string sub, string username, UserRole role, UserStatus status)
    {
        Sub = sub;
        Username = username;
        Role = role;
        Status = status;
    }
}
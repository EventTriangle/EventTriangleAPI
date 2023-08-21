using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.UnitTests.Helpers;

public class UserEntityHelper
{
    private const string CorrectEmail = "user@email.com";
    
    public static UserEntity CreateSuccess()
    {
        return new UserEntity(Guid.NewGuid().ToString(), CorrectEmail, Guid.NewGuid(), UserRole.User, UserStatus.Active);
    }
    
    public static UserEntity CreateWithUserId(string userId)
    {
        return new UserEntity(userId, CorrectEmail, Guid.NewGuid(), UserRole.User, UserStatus.Active);
    }
    
    public static UserEntity CreateWithEmail(string email)
    {
        return new UserEntity(Guid.NewGuid().ToString(), email, Guid.NewGuid(), UserRole.User, UserStatus.Active);
    }
}
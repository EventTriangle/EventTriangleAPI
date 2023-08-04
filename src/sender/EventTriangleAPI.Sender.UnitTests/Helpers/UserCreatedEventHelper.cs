using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public static class UserCreatedEventHelper
{
    private const string CorrectEmail = "user@email.com";
    
    public static UserCreatedEvent CreateSuccess()
    {
        return new UserCreatedEvent(Guid.NewGuid().ToString(), CorrectEmail, UserRole.User, UserStatus.Active);
    }
    
    public static UserCreatedEvent CreateWithUserId(string userId)
    {
        return new UserCreatedEvent(userId, CorrectEmail, UserRole.User, UserStatus.Active);
    }
    
    public static UserCreatedEvent CreateWithEmail(string email)
    {
        return new UserCreatedEvent(Guid.NewGuid().ToString(), email, UserRole.User, UserStatus.Active);
    }
}
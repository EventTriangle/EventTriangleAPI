using EventTriangleAPI.Shared.Domain.Entities;
using EventTriangleAPI.Shared.Domain.Enums;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public static class UserCreatedEventHelper
{
    public static UserCreatedEvent CreateSuccess()
    {
        return new UserCreatedEvent(Guid.NewGuid().ToString(), UserRole.User, UserStatus.Active);
    }
    
    public static UserCreatedEvent CreateWithUserId(string userId)
    {
        return new UserCreatedEvent(userId, UserRole.User, UserStatus.Active);
    }
}
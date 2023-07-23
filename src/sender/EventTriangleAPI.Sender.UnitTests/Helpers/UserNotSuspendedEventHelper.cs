using EventTriangleAPI.Shared.Domain.Entities;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public class UserNotSuspendedEventHelper
{
    public static UserNotSuspendedEvent CreateSuccess()
    {
        return new UserNotSuspendedEvent(Guid.NewGuid().ToString());
    }
    
    public static UserNotSuspendedEvent CreateWithUserId(string userId)
    {
        return new UserNotSuspendedEvent(userId);
    }
}
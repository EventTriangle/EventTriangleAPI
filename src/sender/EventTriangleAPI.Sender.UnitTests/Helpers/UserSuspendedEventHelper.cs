using EventTriangleAPI.Sender.Domain.Entities;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public class UserSuspendedEventHelper
{
    public static UserSuspendedEvent CreateSuccess()
    {
        return new UserSuspendedEvent(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
    }
    
    public static UserSuspendedEvent CreateWithUserId(string userId)
    {
        return new UserSuspendedEvent(Guid.NewGuid().ToString(), userId);
    }
}
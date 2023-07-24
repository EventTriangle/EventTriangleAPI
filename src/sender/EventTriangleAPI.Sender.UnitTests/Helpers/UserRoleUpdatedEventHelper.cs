using EventTriangleAPI.Shared.Domain.Enums;
using EventTriangleAPI.Shared.Domain.Events;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public static class UserRoleUpdatedEventHelper
{
    public static UserRoleUpdatedEvent CreateSuccess()
    {
        return new UserRoleUpdatedEvent(Guid.NewGuid().ToString(), UserRole.Admin);
    }
    
    public static UserRoleUpdatedEvent CreateWithUserId(string userId)
    {
        return new UserRoleUpdatedEvent(userId, UserRole.Admin);
    }
}
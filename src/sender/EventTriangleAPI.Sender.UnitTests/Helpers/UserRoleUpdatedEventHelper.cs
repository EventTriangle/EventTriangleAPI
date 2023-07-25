using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Shared.DTO.Enums;

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
using FluxPayAPI.Sender.Domain.Entities;
using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Sender.UnitTests.Helpers;

public static class UserRoleUpdatedEventHelper
{
    public static UserRoleUpdatedEvent CreateSuccess()
    {
        return new UserRoleUpdatedEvent(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), UserRole.Admin);
    }
    
    public static UserRoleUpdatedEvent CreateWithUserId(string userId)
    {
        return new UserRoleUpdatedEvent(Guid.NewGuid().ToString(), userId, UserRole.Admin);
    }
}
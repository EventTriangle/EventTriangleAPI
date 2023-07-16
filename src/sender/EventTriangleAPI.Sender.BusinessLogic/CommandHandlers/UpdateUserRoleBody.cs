using EventTriangleAPI.Shared.Application.Enums;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record UpdateUserRoleBody(string UserId, UserRole UserRole);
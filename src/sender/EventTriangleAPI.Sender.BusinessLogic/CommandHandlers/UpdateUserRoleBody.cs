using EventTriangleAPI.Shared.Application.Enums;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record UpdateUserRoleBody(Guid UserId, UserRole UserRole);
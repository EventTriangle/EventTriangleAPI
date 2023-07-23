using EventTriangleAPI.Shared.Domain.Enums;
using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record UpdateUserRoleCommand(string UserId, UserRole UserRole) : ICommand;
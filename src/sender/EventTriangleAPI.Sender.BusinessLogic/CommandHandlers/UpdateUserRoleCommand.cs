using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record UpdateUserRoleCommand(string UserId, UserRole UserRole) : ICommand;
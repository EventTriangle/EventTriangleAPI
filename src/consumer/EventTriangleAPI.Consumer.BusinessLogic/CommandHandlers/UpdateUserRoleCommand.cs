using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public record UpdateUserRoleCommand(string RequesterId, string UserId, UserRole UserRole) : ICommand;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record CreateUserCommand(
    string UserId, 
    string Email, 
    UserRole UserRole,
    UserStatus UserStatus) : ICommand;
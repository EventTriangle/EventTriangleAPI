using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public record CreateUserCommand(
    string UserId, 
    string Email, 
    UserRole UserRole,
    UserStatus UserStatus) : ICommand;
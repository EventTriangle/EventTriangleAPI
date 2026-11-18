using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;

public record UpdateUserRoleCommand(string RequesterId, string UserId, UserRole UserRole) : ICommand;
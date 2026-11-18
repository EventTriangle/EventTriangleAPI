using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Sender.BusinessLogic.Models.Requests;

public record UpdateUserRoleRequest(string UserId, UserRole UserRole);
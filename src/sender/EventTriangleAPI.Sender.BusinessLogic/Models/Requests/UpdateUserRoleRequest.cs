using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Sender.BusinessLogic.Models.Requests;

public record UpdateUserRoleRequest(string UserId, UserRole UserRole);
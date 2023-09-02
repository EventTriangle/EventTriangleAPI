using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.BusinessLogic.Models.Notifications;

public record UserRoleUpdatingCanceledNotification(
    string UserId,
    UserRole UserRole,
    string Reason);
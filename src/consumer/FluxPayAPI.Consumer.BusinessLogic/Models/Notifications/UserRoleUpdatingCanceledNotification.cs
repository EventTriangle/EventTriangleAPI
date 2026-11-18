using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Consumer.BusinessLogic.Models.Notifications;

public record UserRoleUpdatingCanceledNotification(
    string UserId,
    UserRole UserRole,
    string Reason);
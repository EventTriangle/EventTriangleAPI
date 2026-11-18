namespace FluxPayAPI.Consumer.BusinessLogic.Models.Notifications;

public record UserSuspendingCanceledNotification(
    string UserId,
    string Reason);
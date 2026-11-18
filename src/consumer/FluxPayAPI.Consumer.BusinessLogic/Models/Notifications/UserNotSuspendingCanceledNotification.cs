namespace FluxPayAPI.Consumer.BusinessLogic.Models.Notifications;

public record UserNotSuspendingCanceledNotification(
    string UserId,
    string Reason);
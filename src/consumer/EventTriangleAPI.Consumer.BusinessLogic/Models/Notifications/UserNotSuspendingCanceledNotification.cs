namespace EventTriangleAPI.Consumer.BusinessLogic.Models.Notifications;

public record UserNotSuspendingCanceledNotification(
    string UserId,
    string Reason);
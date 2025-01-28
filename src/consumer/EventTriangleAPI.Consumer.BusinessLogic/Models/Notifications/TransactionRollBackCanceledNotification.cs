namespace EventTriangleAPI.Consumer.BusinessLogic.Models.Notifications;

public record TransactionRollBackCanceledNotification(
    Guid TransactionId,
    string Reason);
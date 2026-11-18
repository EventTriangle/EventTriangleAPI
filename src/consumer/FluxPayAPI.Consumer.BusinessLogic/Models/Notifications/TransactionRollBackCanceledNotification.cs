namespace FluxPayAPI.Consumer.BusinessLogic.Models.Notifications;

public record TransactionRollBackCanceledNotification(
    Guid TransactionId,
    string Reason);
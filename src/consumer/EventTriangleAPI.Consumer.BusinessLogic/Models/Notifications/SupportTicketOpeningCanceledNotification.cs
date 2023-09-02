namespace EventTriangleAPI.Consumer.BusinessLogic.Models.Notifications;

public record SupportTicketOpeningCanceledNotification(
    Guid WalletId, 
    Guid TransactionId,
    string TicketReason,
    string Reason);
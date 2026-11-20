namespace FluxPayAPI.Consumer.BusinessLogic.Models.Notifications;

public record CreditCardAddedNotice(string UserId, Guid CardId);
public record SupportTicketOpenedNotice(string UserId, SupportTicketDto Ticket);
public record SupportTicketResolvedNotice(string UserId, SupportTicketDto Ticket);
public record TransactionSucceededNotice(string RequesterId, string ToUserId, TransactionDto Transaction, Guid? OperationId);
public record TransactionCanceledNotice(string RequesterId, TransactionCanceledNotification Notification);

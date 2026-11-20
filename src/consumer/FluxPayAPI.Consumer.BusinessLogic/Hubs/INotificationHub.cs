using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.BusinessLogic.Models.Notifications;

namespace FluxPayAPI.Consumer.BusinessLogic.Hubs;

public interface INotificationHub
{
    //success 
    Task TransactionSucceededAsync(TransactionDto transaction, Guid? operationId);
    
    Task CreditCardAddedAsync(CreditCardDto creditCardDto);

    Task SupportTicketOpenedAsync(SupportTicketDto supportTicketDto);

    Task SupportTicketResolvedAsync(SupportTicketDto supportTicketDto);
    
    //fail
    Task ContactCreatingCanceledAsync(ContactCreatingCanceledNotification notification);

    Task ContactDeletingCanceledAsync(ContactDeletingCanceledNotification notification);

    Task CreditCardAddingCanceledAsync(CreditCardAddingCanceledNotification notification);
    
    Task CreditCardChangingCanceledAsync(CreditCardChangingCanceledNotification notification);
    
    Task CreditCardDeletingCanceledAsync(CreditCardDeletingCanceledNotification notification);
    
    Task SupportTicketOpeningCanceledAsync(SupportTicketOpeningCanceledNotification notification);

    Task SupportTicketResolvingCanceledAsync(SupportTicketResolvingCanceledNotification notification);
    
    Task TransactionRollBackCanceledAsync(TransactionRollBackCanceledNotification notification);

    Task UserNotSuspendingCanceledAsync(UserNotSuspendingCanceledNotification notification);
    
    Task UserSuspendingCanceledAsync(UserSuspendingCanceledNotification notification);

    Task UserRoleUpdatingCanceledAsync(UserRoleUpdatingCanceledNotification notification);
    
    Task TransactionCanceledAsync(TransactionCanceledNotification notification);
}
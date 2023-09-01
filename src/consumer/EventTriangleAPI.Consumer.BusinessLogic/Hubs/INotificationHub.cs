using EventTriangleAPI.Consumer.BusinessLogic.Models.Notifications;

namespace EventTriangleAPI.Consumer.BusinessLogic.Hubs;

public interface INotificationHub
{
    Task TransactionCanceledAsync(TransactionCanceledNotification notification);
}
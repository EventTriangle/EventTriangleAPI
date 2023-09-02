namespace EventTriangleAPI.Consumer.BusinessLogic.Models.Notifications;

public record CreditCardDeletingCanceledNotification(
    Guid CardId,
    string Reason);
namespace EventTriangleAPI.Consumer.BusinessLogic.Models.Notifications;

public record ContactDeletingCanceledNotification(string ContactId, string Reason);
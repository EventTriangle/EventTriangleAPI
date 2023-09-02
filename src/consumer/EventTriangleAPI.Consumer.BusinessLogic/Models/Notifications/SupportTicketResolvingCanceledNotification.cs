namespace EventTriangleAPI.Consumer.BusinessLogic.Models.Notifications;

public record SupportTicketResolvingCanceledNotification(
    Guid TicketId, 
    string TicketJustification,
    string Reason);
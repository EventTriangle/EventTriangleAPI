namespace EventTriangleAPI.Sender.BusinessLogic.Models.Requests;

public record ResolveSupportTicketRequest(
    Guid TicketId,
    string TicketJustification);
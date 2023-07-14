namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record ResolveSupportTicketBody(Guid TicketId, string TicketJustification);
using EventTriangleAPI.Sender.Domain.Entities;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public static class SupportTicketResolvedEventHelper
{
    private const string TicketJustification = "Hello, we've rolled back your transaction.";
    
    public static SupportTicketResolvedEvent CreateSuccess()
    {
        return new SupportTicketResolvedEvent(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            TicketJustification);
    }

    public static SupportTicketResolvedEvent CreateWithTicketId(Guid ticketId)
    {
        return new SupportTicketResolvedEvent(
            Guid.NewGuid().ToString(),
            ticketId,
            TicketJustification);
    }
    
    public static SupportTicketResolvedEvent CreateWithTicketJustification(string ticketJustification)
    {
        return new SupportTicketResolvedEvent(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            ticketJustification);
    }
}
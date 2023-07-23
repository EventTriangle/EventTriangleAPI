using EventTriangleAPI.Shared.Domain.Entities;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public static class SupportTicketResolvedEventHelper
{
    private const string TicketJustification = "Hello, we've rolled back your transaction.";
    
    public static SupportTicketResolvedEvent CreateSuccess()
    {
        return new SupportTicketResolvedEvent(
            Guid.NewGuid(),
            TicketJustification);
    }

    public static SupportTicketResolvedEvent CreateWithTicketId(Guid ticketId)
    {
        return new SupportTicketResolvedEvent(
            ticketId,
            TicketJustification);
    }
    
    public static SupportTicketResolvedEvent CreateWithTicketJustification(string ticketJustification)
    {
        return new SupportTicketResolvedEvent(
            Guid.NewGuid(),
            ticketJustification);
    }
}
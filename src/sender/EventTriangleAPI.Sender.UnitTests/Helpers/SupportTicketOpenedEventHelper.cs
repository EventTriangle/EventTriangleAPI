using EventTriangleAPI.Sender.Domain.Entities;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public static class SupportTicketOpenedEventHelper
{
    private const string TicketReason = "I'm sorry, I'd like to cancel the transaction.";
    
    public static SupportTicketOpenedEvent CreateSuccess()
    {
        return new SupportTicketOpenedEvent(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            TicketReason);
    }
    
    public static SupportTicketOpenedEvent CreateWithUserId(string userId)
    {
        return new SupportTicketOpenedEvent(
            userId,
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            TicketReason);
    }
    
    public static SupportTicketOpenedEvent CreateWithUsername(string username)
    {
        return new SupportTicketOpenedEvent(
            Guid.NewGuid().ToString(),
            username,
            Guid.NewGuid(),
            TicketReason);
    }
    
    public static SupportTicketOpenedEvent CreateWithWalletId(Guid walletId)
    {
        return new SupportTicketOpenedEvent(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            walletId,
            TicketReason);
    }
    
    public static SupportTicketOpenedEvent CreateWithTicketReason(string ticketReason)
    {
        return new SupportTicketOpenedEvent(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            ticketReason);
    }
}
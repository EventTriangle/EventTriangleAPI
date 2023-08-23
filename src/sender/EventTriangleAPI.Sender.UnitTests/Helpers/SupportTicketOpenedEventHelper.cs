using EventTriangleAPI.Sender.Domain.Entities;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public static class SupportTicketOpenedEventHelper
{
    private const string TicketReason = "I'm sorry, I'd like to cancel the transaction.";
    
    public static SupportTicketOpenedEvent CreateSuccess()
    {
        return new SupportTicketOpenedEvent(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            TicketReason);
    }
    
    public static SupportTicketOpenedEvent CreateWithUserId(string userId)
    {
        return new SupportTicketOpenedEvent(
            userId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            TicketReason);
    }
    
    public static SupportTicketOpenedEvent CreateWithWalletId(Guid walletId)
    {
        return new SupportTicketOpenedEvent(
            Guid.NewGuid().ToString(),
            walletId,
            Guid.NewGuid(),
            TicketReason);
    }
    
    public static SupportTicketOpenedEvent CreateWithTransactionId(Guid transactionId)
    {
        return new SupportTicketOpenedEvent(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            transactionId,
            TicketReason);
    }
    
    public static SupportTicketOpenedEvent CreateWithTicketReason(string ticketReason)
    {
        return new SupportTicketOpenedEvent(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            ticketReason);
    }
}
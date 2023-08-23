using EventTriangleAPI.Consumer.Domain.Entities;

namespace EventTriangleAPI.Consumer.UnitTests.Helpers;

public class SupportTicketEntityHelper
{
    private const string TicketReason = "I'm sorry, I'd like to cancel the transaction.";
    
    public static SupportTicketEntity CreateSuccess()
    {
        return new SupportTicketEntity(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            TicketReason,
            DateTime.UtcNow);
    }
    
    public static SupportTicketEntity CreateWithUserId(string userId)
    {
        return new SupportTicketEntity(
            userId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            TicketReason,
            DateTime.UtcNow);
    }
    
    public static SupportTicketEntity CreateWithWalletId(Guid walletId)
    {
        return new SupportTicketEntity(
            Guid.NewGuid().ToString(),
            walletId,
            Guid.NewGuid(),
            TicketReason,
            DateTime.UtcNow);
    }
    
    public static SupportTicketEntity CreateWithTransactionId(Guid transactionId)
    {
        return new SupportTicketEntity(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            transactionId,
            TicketReason,
            DateTime.UtcNow);
    }
    
    public static SupportTicketEntity CreateWithTicketReason(string ticketReason)
    {
        return new SupportTicketEntity(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            ticketReason,
            DateTime.UtcNow);
    }
    
    public static SupportTicketEntity CreateWithTicketJustification(string ticketJustification)
    {
        var ticket = new SupportTicketEntity(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            TicketReason,
            DateTime.UtcNow);

        ticket.UpdateTicketJustification(ticketJustification);
        
        return ticket;
    }
}
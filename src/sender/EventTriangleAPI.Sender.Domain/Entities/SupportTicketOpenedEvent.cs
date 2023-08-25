using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class SupportTicketOpenedEvent
{
    public Guid Id { get; private set; }
    
    public string RequesterId { get; private set; }
    
    public Guid WalletId { get; private set; }
    
    public Guid TransactionId { get; private set; }
    
    public string TicketReason { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public SupportTicketOpenedEvent(string requesterId, Guid walletId, Guid transactionId, string ticketReason)
    {
        Id = Guid.NewGuid();
        RequesterId = requesterId;
        WalletId = walletId;
        TransactionId = transactionId;
        TicketReason = ticketReason;
        CreatedAt = DateTime.UtcNow;
        
        new SupportTicketOpenedEventValidator().ValidateAndThrow(this);
    }

    public SupportTicketOpenedEventMessage CreateEventMessage()
    {
        return new SupportTicketOpenedEventMessage(Id, RequesterId, WalletId, TransactionId, TicketReason, CreatedAt);
    }
}
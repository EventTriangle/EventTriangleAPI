using EventTriangleAPI.Shared.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Entities;

public class SupportTicketOpenedEvent
{
    public Guid Id { get; private set; }
    
    public string UserId { get; private set; }
    
    public string Username { get; private set; }
    
    public Guid WalletId { get; private set; }
    
    public string TicketReason { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public SupportTicketOpenedEvent(string userId, string username, Guid walletId, string ticketReason)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Username = username;
        WalletId = walletId;
        TicketReason = ticketReason;
        CreatedAt = DateTime.UtcNow;
        
        new SupportTicketOpenedEventValidator().ValidateAndThrow(this);
    }
}
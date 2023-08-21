using EventTriangleAPI.Consumer.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentValidation;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class SupportTicketEntity
{
    public Guid Id { get; private set; }

    public string UserId { get; private set; }

    public UserEntity User { get; private set; }
    
    public Guid WalletId { get; private set; }
    
    public WalletEntity Wallet { get; private set; }
    
    public string TicketReason { get; private set; }
    
    public string TicketJustification { get; private set; }

    public TicketStatus TicketStatus { get; private set; }

    public SupportTicketEntity(string userId, Guid walletId, string ticketReason)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        WalletId = walletId;
        TicketReason = ticketReason;
        TicketStatus = TicketStatus.Open;
        
        new SupportTicketEntityValidator().ValidateAndThrow(this);
    }

    public void UpdateTicketJustification(string ticketJustification)
    {
        TicketJustification = ticketJustification;
        
        new SupportTicketEntityValidator().ValidateAndThrow(this);
    }
    
    public void UpdateTicketStatus(TicketStatus ticketStatus)
    {
        TicketStatus = ticketStatus;
        
        new SupportTicketEntityValidator().ValidateAndThrow(this);
    }
}
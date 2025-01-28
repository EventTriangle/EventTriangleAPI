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
    
    public Guid TransactionId { get; private set;  }
    
    public TransactionEntity Transaction { get; private set; }
    
    public WalletEntity Wallet { get; private set; }
    
    public string TicketReason { get; private set; }
    
    public string TicketJustification { get; private set; }

    public TicketStatus TicketStatus { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    private static readonly SupportTicketEntityValidator Validator = new(); 
    
    public SupportTicketEntity(string userId, Guid walletId, Guid transactionId, string ticketReason, DateTime createdAt)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        WalletId = walletId;
        TransactionId = transactionId;
        TicketReason = ticketReason;
        TicketStatus = TicketStatus.Open;
        CreatedAt = createdAt;
        
        Validator.ValidateAndThrow(this);
    }

    public void UpdateTicketJustification(string ticketJustification)
    {
        TicketJustification = ticketJustification;
        
        Validator.ValidateAndThrow(this);
    }
    
    public void UpdateTicketStatus(TicketStatus ticketStatus)
    {
        TicketStatus = ticketStatus;
        
        Validator.ValidateAndThrow(this);
    }
}
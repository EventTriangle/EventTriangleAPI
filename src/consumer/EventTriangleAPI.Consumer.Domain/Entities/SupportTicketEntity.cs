using EventTriangleAPI.Consumer.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentValidation;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class SupportTicketEntity
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Guid WalletId { get; private set; }
    
    public string TicketReason { get; private set; }
    
    public string TicketJustification { get; private set; }

    public TicketStatus TicketStatus { get; private set; } = TicketStatus.Open;

    public SupportTicketEntity(Guid userId, Guid walletId, string ticketReason)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        WalletId = walletId;
        TicketReason = ticketReason;
        
        new SupportTicketEntityValidator().ValidateAndThrow(this);
    }
}
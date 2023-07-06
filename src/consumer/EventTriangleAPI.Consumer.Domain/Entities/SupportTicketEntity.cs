using EventTriangleAPI.Consumer.Domain.Entities.Validation;
using EventTriangleAPI.Shared.Application.Enums;
using FluentValidation;
using Uuids;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class SupportTicketEntity
{
    public Guid Id { get; private set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();

    public Guid UserId { get; private set; }

    public Guid WalletId { get; private set; }
    
    public string TicketReason { get; private set; }
    
    public string TicketJustification { get; private set; }

    public TicketStatus TicketStatus { get; private set; } = TicketStatus.Open;

    public SupportTicketEntity(Guid userId, Guid walletId, string ticketReason)
    {
        UserId = userId;
        WalletId = walletId;
        TicketReason = ticketReason;
        
        new SupportTicketEntityValidator().ValidateAndThrow(this);
    }
}
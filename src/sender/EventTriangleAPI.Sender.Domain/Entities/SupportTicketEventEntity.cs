using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.Application.Enums;
using FluentValidation;
using Uuids;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class SupportTicketEventEntity
{
    public Guid Id { get; private set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();
    
    public Guid UserId { get; private set; }
    
    public string Username { get; private set; }

    public Guid WalletId { get; private set; }
    
    public string TicketReason { get; private set; }
    
    public TicketStatus TicketStatus { get; private set; }

    public SupportTicketEventEntity(
        Guid userId, 
        string username, 
        Guid walletId, 
        string ticketReason, 
        TicketStatus ticketStatus)
    {
        UserId = userId;
        Username = username;
        WalletId = walletId;
        TicketReason = ticketReason;
        TicketStatus = ticketStatus;

        new SupportTicketEventEntityValidator().ValidateAndThrow(this);
    }
}
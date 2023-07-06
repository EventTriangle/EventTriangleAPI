using EventTriangleAPI.Shared.Application.Enums;
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
}
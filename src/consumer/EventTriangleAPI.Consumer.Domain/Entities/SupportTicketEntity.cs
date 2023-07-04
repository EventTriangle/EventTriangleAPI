using EventTriangleAPI.Consumer.Domain.Enums;
using Uuids;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class SupportTicketEntity
{
    public Guid Id { get; set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();

    public Guid UserId { get; set; }

    public Guid WalletId { get; set; }
    
    public string TicketReason { get; set; }
    
    public string TicketJustification { get; set; }
    
    public TicketStatus TicketStatus { get; set; }
}
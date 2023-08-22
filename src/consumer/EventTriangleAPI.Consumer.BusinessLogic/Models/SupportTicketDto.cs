using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.BusinessLogic.Models;

public class SupportTicketDto
{
    public Guid Id { get; set; }

    public string UserId { get; set; }

    public Guid WalletId { get; set; }
    
    public string TicketReason { get; set; }
    
    public string TicketJustification { get; set; }

    public TicketStatus TicketStatus { get; set; }
}
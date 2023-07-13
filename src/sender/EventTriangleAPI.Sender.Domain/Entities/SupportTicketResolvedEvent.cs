using EventTriangleAPI.Sender.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class SupportTicketResolvedEvent
{
    public Guid Id { get; private set; }
    
    public Guid TicketId { get; private set; }
    
    public string TicketJustification { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public SupportTicketResolvedEvent(Guid ticketId, string ticketJustification)
    {
        Id = Guid.NewGuid();
        TicketId = ticketId;
        TicketJustification = ticketJustification;
        CreatedAt = DateTime.UtcNow;
        
        new SupportTicketResolvedEventValidator().ValidateAndThrow(this);
    }
}
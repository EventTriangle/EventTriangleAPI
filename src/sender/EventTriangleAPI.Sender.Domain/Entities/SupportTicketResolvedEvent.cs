using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class SupportTicketResolvedEvent
{
    public Guid Id { get; private set; }
    
    public string RequesterId { get; private set; }
    
    public Guid TicketId { get; private set; }
    
    public string TicketJustification { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    private static readonly SupportTicketResolvedEventValidator Validator = new(); 
    
    public SupportTicketResolvedEvent(string requesterId, Guid ticketId, string ticketJustification)
    {
        Id = Guid.NewGuid();
        RequesterId = requesterId;
        TicketId = ticketId;
        TicketJustification = ticketJustification;
        CreatedAt = DateTime.UtcNow;
        
        Validator.ValidateAndThrow(this);
    }

    public SupportTicketResolvedEventMessage CreateEventMessage()
    {
        return new SupportTicketResolvedEventMessage(Id, RequesterId, TicketId, TicketJustification, CreatedAt);
    }
}
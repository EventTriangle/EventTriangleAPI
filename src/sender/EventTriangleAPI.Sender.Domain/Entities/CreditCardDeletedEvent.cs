using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class CreditCardDeletedEvent
{
    public Guid Id { get; private set; }
    
    public string RequesterId { get; private set; }
    
    public Guid CardId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public CreditCardDeletedEvent(string requesterId, Guid cardId)
    {
        Id = Guid.NewGuid();
        RequesterId = requesterId;
        CardId = cardId;
        CreatedAt = DateTime.UtcNow;
        
        new CreditCardDeletedEventValidator().ValidateAndThrow(this);
    }

    public CreditCardDeletedEventMessage CreateEventMessage()
    {
        return new CreditCardDeletedEventMessage(Id, RequesterId, CardId, CreatedAt);
    }
}
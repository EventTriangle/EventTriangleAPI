using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class CreditCardDeletedEvent
{
    public Guid Id { get; private set; }
    
    public string UserId { get; private set; }
    
    public Guid CardId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public CreditCardDeletedEvent(string userId, Guid cardId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CardId = cardId;
        CreatedAt = DateTime.UtcNow;
        
        new CreditCardDeletedEventValidator().ValidateAndThrow(this);
    }

    public CreditCardDeletedEventMessage CreateEventMessage()
    {
        return new CreditCardDeletedEventMessage(Id, UserId, CardId, CreatedAt);
    }
}
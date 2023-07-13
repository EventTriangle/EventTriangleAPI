using EventTriangleAPI.Sender.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class CreditCardDeletedEvent
{
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public Guid CardId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public CreditCardDeletedEvent(Guid userId, Guid cardId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CardId = cardId;
        CreatedAt = DateTime.UtcNow;
        
        new CreditCardDeletedEventValidator().ValidateAndThrow(this);
    }
}
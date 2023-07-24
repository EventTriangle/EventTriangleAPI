using EventTriangleAPI.Shared.Domain.Events.Validation;
using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Events;

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
}
using EventTriangleAPI.Shared.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Entities;

public class TransactionRollBackedEvent
{
    public Guid Id { get; private set; }
    
    public Guid TransactionId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public TransactionRollBackedEvent(Guid transactionId)
    {
        Id = Guid.NewGuid();
        TransactionId = transactionId;
        CreatedAt = DateTime.UtcNow;
        
        new TransactionRollBackedEventValidator().ValidateAndThrow(this);
    }
}
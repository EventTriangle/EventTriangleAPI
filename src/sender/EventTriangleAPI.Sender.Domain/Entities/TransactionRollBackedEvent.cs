using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

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

    public TransactionRollBackedEventMessage CreateEventMessage()
    {
        return new TransactionRollBackedEventMessage(Id, TransactionId, CreatedAt);
    }
}
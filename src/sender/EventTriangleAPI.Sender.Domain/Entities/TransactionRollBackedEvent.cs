using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class TransactionRollBackedEvent
{
    public Guid Id { get; private set; }
    
    public string RequesterId { get; private set; }
    
    public Guid TransactionId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    private static readonly TransactionRollBackedEventValidator Validator = new(); 

    public TransactionRollBackedEvent(string requesterId, Guid transactionId)
    {
        Id = Guid.NewGuid();
        RequesterId = requesterId;
        TransactionId = transactionId;
        CreatedAt = DateTime.UtcNow;
        
        Validator.ValidateAndThrow(this);
    }

    public TransactionRollBackedEventMessage CreateEventMessage()
    {
        return new TransactionRollBackedEventMessage(Id, RequesterId, TransactionId, CreatedAt);
    }
}
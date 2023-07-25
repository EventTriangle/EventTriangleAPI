using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class TransactionCreatedEvent
{
    public Guid Id { get; private set; }
    
    public string From { get; private set; }
    
    public string To { get; private set; }
    
    public decimal Amount { get; private set; }
    
    public TransactionType TransactionType { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public TransactionCreatedEvent(string from, string to, decimal amount, TransactionType transactionType)
    {
        Id = Guid.NewGuid();
        From = from;
        To = to;
        Amount = amount;
        TransactionType = transactionType;
        CreatedAt = DateTime.UtcNow;
        
        new TransactionCreatedEventValidator().ValidateAndThrow(this);
    }

    public TransactionCreatedEventMessage CreateEventMessage()
    {
        return new TransactionCreatedEventMessage(Id, From, To, Amount, TransactionType, CreatedAt);
    }
}
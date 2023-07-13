using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.Application.Enums;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class TransactionCreatedEvent
{
    public Guid Id { get; private set; }
    
    public string From { get; private set; }
    
    public string To { get; private set; }
    
    public decimal Amount { get; private set; }
    
    public TransactionType TransactionType { get; private set; }

    public DateTime CreateAt { get; private set; }

    public TransactionCreatedEvent(string from, string to, decimal amount, TransactionType transactionType)
    {
        Id = Guid.NewGuid();
        From = from;
        To = to;
        Amount = amount;
        TransactionType = transactionType;
        CreateAt = DateTime.UtcNow;
        
        new TransactionCreatedEventValidator().ValidateAndThrow(this);
    }
}
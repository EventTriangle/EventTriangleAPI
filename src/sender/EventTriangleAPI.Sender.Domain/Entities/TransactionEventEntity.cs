using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.Application.Enums;
using FluentValidation;
using Uuids;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class TransactionEventEntity
{
    public Guid Id { get; private set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();
    
    public string From { get; private set; }
    
    public string To { get; private set; }
    
    public decimal Amount { get; private set; }
    
    public TransactionType TransactionType { get; private set; }

    public DateTime CreateAt { get; private set; } = DateTime.UtcNow;

    public TransactionEventEntity(string from, string to, decimal amount, TransactionType transactionType)
    {
        From = from;
        To = to;
        Amount = amount;
        TransactionType = transactionType;
        
        new TransactionEventEntityValidator().ValidateAndThrow(this);
    }
}
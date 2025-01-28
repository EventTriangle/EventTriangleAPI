using EventTriangleAPI.Consumer.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentValidation;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class TransactionEntity
{
    public Guid Id { get; private set; }
    
    public string FromUserId { get; private set; }
    
    public UserEntity FromUser { get; private set; }
    
    public string ToUserId { get; private set; }
    
    public UserEntity ToUser { get; private set; }
    
    public decimal Amount { get; private set; }

    public TransactionState TransactionState { get; private set; }
    
    public TransactionType TransactionType { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private static readonly TransactionEntityValidator Validator = new(); 
    
    public TransactionEntity(
        string fromUserId, 
        string toUserId, 
        decimal amount, 
        TransactionType transactionType,
        DateTime createdAt)
    {
        Id = Guid.NewGuid();
        FromUserId = fromUserId;
        ToUserId = toUserId;
        Amount = amount;
        TransactionState = TransactionState.Completed;
        TransactionType = transactionType;
        CreatedAt = createdAt;
        
        Validator.ValidateAndThrow(this);
    }

    public void UpdateTransactionState(TransactionState transactionState)
    {
        TransactionState = transactionState;
        
        Validator.ValidateAndThrow(this);
    }
}
using EventTriangleAPI.Consumer.Domain.Entities.Validation;
using FluentValidation;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class WalletEntity
{
    public Guid Id { get; private set; }
    
    public UserEntity User { get; private set; }
    
    public decimal Balance { get; private set; }

    public Guid? LastTransactionId { get; private set; }
    
    public TransactionEntity LastTransaction { get; private set; }

    private static readonly WalletEntityValidator Validator = new(); 
    
    public WalletEntity(decimal balance)
    {
        Id = Guid.NewGuid();
        Balance = balance;
        
        Validator.ValidateAndThrow(this);
    }

    public void UpdateBalance(decimal balance)
    {
        Balance = balance;
        
        Validator.ValidateAndThrow(this);
    }
}
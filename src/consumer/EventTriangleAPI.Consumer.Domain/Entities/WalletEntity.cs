using EventTriangleAPI.Consumer.Domain.Entities.Validation;
using FluentValidation;
using Uuids;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class WalletEntity
{
    public Guid Id { get; private set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();
    
    public Guid UserId { get; private set; }
    
    public decimal Balance { get; private set; }

    public Guid? LastTransactionId { get; private set; }
    
    public TransactionEntity LastTransaction { get; private set; }

    public WalletEntity(Guid userId, decimal balance)
    {
        UserId = userId;
        Balance = balance;
        
        new WalletEntityValidator().ValidateAndThrow(this);
    }
}
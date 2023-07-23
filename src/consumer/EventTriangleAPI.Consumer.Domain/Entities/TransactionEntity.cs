using EventTriangleAPI.Shared.Domain.Enums;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class TransactionEntity
{
    public Guid Id { get; private set; }
    
    public string From { get; private set; }
    
    public string To { get; private set; }
    
    public Guid FromWalletId { get; private set; }
    
    public WalletEntity FromWallet { get; private set; }
    
    public Guid ToWalletId { get; private set; }
    
    public WalletEntity ToWallet { get; private set; }
    
    public decimal Amount { get; private set; }

    public TransactionState TransactionState { get; private set; } = TransactionState.Completed;
    
    public TransactionType TransactionType { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public TransactionEntity(string from, string to, Guid fromWalletId, Guid toWalletId, TransactionType transactionType)
    {
        From = from;
        To = to;
        FromWalletId = fromWalletId;
        ToWalletId = toWalletId;
        TransactionType = transactionType;
    }
}
using EventTriangleAPI.Shared.DTO.Enums;

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

    public TransactionEntity(string fromUserId, string toUserId, TransactionType transactionType)
    {
        FromUserId = fromUserId;
        ToUserId = toUserId;
        TransactionState = TransactionState.Completed;
        TransactionType = transactionType;
        CreatedAt = DateTime.UtcNow;
    }
}
using EventTriangleAPI.Consumer.Domain.Enums;
using Uuids;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class TransactionEntity
{
    public Guid Id { get; set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();
    
    public string From { get; set; }
    
    public string To { get; set; }
    
    public Guid FromWalletId { get; set; }
    
    public Guid ToWalletId { get; set; }
    
    public decimal Amount { get; set; }
    
    public TransactionState TransactionState { get; set; }
    
    public TransactionType TransactionType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
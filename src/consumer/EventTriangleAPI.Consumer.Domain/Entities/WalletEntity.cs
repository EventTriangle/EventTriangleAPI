using Uuids;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class WalletEntity
{
    public Guid Id { get; set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();
    
    public Guid UserId { get; set; }
    
    public decimal Balance { get; set; }
    
    public DateTime LastTransactionDate { get; set; }
    
    public Guid LastTransactionId { get; set; }
}
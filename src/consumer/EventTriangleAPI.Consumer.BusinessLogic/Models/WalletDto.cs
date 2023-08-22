namespace EventTriangleAPI.Consumer.BusinessLogic.Models;

public class WalletDto
{
    public Guid Id { get; set; }
    
    public decimal Balance { get; set; }

    public Guid? LastTransactionId { get; set; }
}
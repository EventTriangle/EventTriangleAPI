using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.BusinessLogic.Models;

public class TransactionDto
{
    public Guid Id { get; set; }
    
    public string FromUserId { get; set; }
    
    public string ToUserId { get; set; }
    
    public decimal Amount { get; set; }

    public TransactionState TransactionState { get; set; }
    
    public TransactionType TransactionType { get; set; }

    public DateTime CreatedAt { get; set; }
}
using EventTriangleAPI.Consumer.Domain.Enums;
using Uuids;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class CreditCardEntity
{
    public Guid Id { get; set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();
    
    public Guid UserId { get; set; }

    public string HolderName { get; set; }

    public string CardNumber { get; set; }
    
    public string Cvv { get; set; }
    
    public PaymentNetwork PaymentNetwork { get; set; }
}
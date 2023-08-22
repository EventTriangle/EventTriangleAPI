using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.BusinessLogic.Models;

public class CreditCardDto
{
    public Guid Id { get; set; }
    
    public string UserId { get; set; }
    
    public string HolderName { get; set; }

    public string CardNumber { get; set; }
    
    public string Cvv { get; set; }
    
    public string Expiration { get; set; }
    
    public PaymentNetwork PaymentNetwork { get; set; }
}
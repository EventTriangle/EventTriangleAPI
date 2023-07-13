using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.Application.Enums;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class CreditCardAddedEvent
{
    public Guid Id { get; private set; }
    
    public Guid CardId { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public string HolderName { get; private set; }
    
    public string CardNumber { get; private set; }
    
    public string Cvv { get; private set; }
    
    public string Expiration { get; private set; }
    
    public PaymentNetwork PaymentNetwork { get; private set;  }
    
    public DateTime CreatedAt { get; private set;  }

    public CreditCardAddedEvent(
        Guid cardId, 
        Guid userId, 
        string holderName, 
        string cardNumber,
        string cvv,
        string expiration,
        PaymentNetwork paymentNetwork)
    {
        Id = Guid.NewGuid();
        CardId = cardId;
        UserId = userId;
        HolderName = holderName;
        CardNumber = cardNumber;
        Cvv = cvv;
        PaymentNetwork = paymentNetwork;
        Expiration = expiration;
        CreatedAt = DateTime.UtcNow;

        new CreditCardAddedEventValidator().ValidateAndThrow(this);
    }
}
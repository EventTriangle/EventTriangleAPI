using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class CreditCardAddedEvent
{
    public Guid Id { get; private set; }
    
    public string RequesterId { get; private set; }
    
    public string HolderName { get; private set; }
    
    public string CardNumber { get; private set; }
    
    public string Cvv { get; private set; }
    
    public string Expiration { get; private set; }
    
    public PaymentNetwork PaymentNetwork { get; private set;  }
    
    public DateTime CreatedAt { get; private set;  }

    public CreditCardAddedEvent(
        string requesterId, 
        string holderName, 
        string cardNumber,
        string cvv,
        string expiration,
        PaymentNetwork paymentNetwork)
    {
        Id = Guid.NewGuid();
        RequesterId = requesterId;
        HolderName = holderName;
        CardNumber = cardNumber;
        Cvv = cvv;
        PaymentNetwork = paymentNetwork;
        Expiration = expiration;
        CreatedAt = DateTime.UtcNow;

        new CreditCardAddedEventValidator().ValidateAndThrow(this);
    }

    public CreditCardAddedEventMessage CreateEventMessage()
    {
        return new CreditCardAddedEventMessage(Id, RequesterId, HolderName, CardNumber, Cvv, Expiration, PaymentNetwork, CreatedAt);
    }
}
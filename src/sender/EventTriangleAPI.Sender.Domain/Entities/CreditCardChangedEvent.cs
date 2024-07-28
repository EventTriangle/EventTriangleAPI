using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class CreditCardChangedEvent
{
    public Guid Id { get; private set; }
    
    public Guid CardId { get; private set; }
    
    public string RequesterId { get; private set; }
    
    public string HolderName { get; private set; }
    
    public string CardNumber { get; private set; }
    
    public string Cvv { get; private set; }
    
    public string Expiration { get; private set; }
    
    public PaymentNetwork PaymentNetwork { get; private set;  }
    
    public DateTime CreatedAt { get; private set;  }
    
    private static readonly CreditCardChangedEventValidator Validator = new(); 
    
    public CreditCardChangedEvent(
        Guid cardId, 
        string requesterId, 
        string holderName, 
        string cardNumber,
        string cvv,
        string expiration,
        PaymentNetwork paymentNetwork)
    {
        Id = Guid.NewGuid();
        CardId = cardId;
        RequesterId = requesterId;
        HolderName = holderName;
        CardNumber = cardNumber;
        Cvv = cvv;
        PaymentNetwork = paymentNetwork;
        Expiration = expiration;
        CreatedAt = DateTime.UtcNow;

        Validator.ValidateAndThrow(this);
    }
    
    public CreditCardChangedEventMessage CreateEventMessage()
    {
        return new CreditCardChangedEventMessage(Id, CardId, RequesterId, HolderName, CardNumber, Cvv, Expiration, PaymentNetwork, CreatedAt);
    }
}
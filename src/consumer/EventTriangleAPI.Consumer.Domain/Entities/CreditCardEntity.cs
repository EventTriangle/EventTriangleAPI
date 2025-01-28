using EventTriangleAPI.Consumer.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentValidation;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class CreditCardEntity
{
    public Guid Id { get; private set; }
    
    public string UserId { get; private set; }

    public UserEntity User { get; private set; }
    
    public string HolderName { get; private set; }

    public string CardNumber { get; private set; }
    
    public string Cvv { get; private set; }
    
    public string Expiration { get; private set; }
    
    public PaymentNetwork PaymentNetwork { get; private set; }

    private static readonly CreditCardEntityValidator Validator = new(); 
    
    public CreditCardEntity(string userId, string holderName, string cardNumber, string cvv, string expiration, PaymentNetwork paymentNetwork)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        HolderName = holderName;
        CardNumber = cardNumber;
        Cvv = cvv;
        Expiration = expiration;
        PaymentNetwork = paymentNetwork;
        
        Validator.ValidateAndThrow(this);
    }

    public void UpdateHolderName(string holderName)
    {
        HolderName = holderName;
        
        Validator.ValidateAndThrow(this);
    }
    
    public void UpdateCardNumber(string cardNumber)
    {
        CardNumber = cardNumber;
        
        Validator.ValidateAndThrow(this);
    }
    
    public void UpdateCvv(string cvv)
    {
        Cvv = cvv;
        
        Validator.ValidateAndThrow(this);
    }

    public void UpdateExpiration(string expiration)
    {
        Expiration = expiration;
        
        Validator.ValidateAndThrow(this);
    }
    
    public void UpdatePaymentNetwork(PaymentNetwork paymentNetwork)
    {
        PaymentNetwork = paymentNetwork;
        
        Validator.ValidateAndThrow(this);
    }
}
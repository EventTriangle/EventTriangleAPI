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
    
    public PaymentNetwork PaymentNetwork { get; private set; }

    public CreditCardEntity(string userId, string holderName, string cardNumber, string cvv, PaymentNetwork paymentNetwork)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        HolderName = holderName;
        CardNumber = cardNumber;
        Cvv = cvv;
        PaymentNetwork = paymentNetwork;
        
        new CreditCardEntityValidator().ValidateAndThrow(this);
    }

    public void UpdateHolderName(string holderName)
    {
        HolderName = holderName;
        
        new CreditCardEntityValidator().ValidateAndThrow(this);
    }
    
    public void UpdateCardNumber(string cardNumber)
    {
        CardNumber = CardNumber;
        
        new CreditCardEntityValidator().ValidateAndThrow(this);
    }
    
    public void UpdateCvv(string cvv)
    {
        Cvv = cvv;
        
        new CreditCardEntityValidator().ValidateAndThrow(this);
    }
    
    public void UpdatePaymentNetwork(PaymentNetwork paymentNetwork)
    {
        PaymentNetwork = paymentNetwork;
        
        new CreditCardEntityValidator().ValidateAndThrow(this);
    }
}
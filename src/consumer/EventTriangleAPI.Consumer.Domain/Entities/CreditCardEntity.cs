using EventTriangleAPI.Consumer.Domain.Entities.Validation;
using EventTriangleAPI.Shared.Domain.Enums;
using FluentValidation;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class CreditCardEntity
{
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }

    public string HolderName { get; private set; }

    public string CardNumber { get; private set; }
    
    public string Cvv { get; private set; }
    
    public PaymentNetwork PaymentNetwork { get; set; }

    public CreditCardEntity(Guid userId, string holderName, string cardNumber, string cvv, PaymentNetwork paymentNetwork)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        HolderName = holderName;
        CardNumber = cardNumber;
        Cvv = cvv;
        PaymentNetwork = paymentNetwork;
        
        new CreditCardEntityValidator().ValidateAndThrow(this);
    }
}
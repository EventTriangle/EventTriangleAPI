using EventTriangleAPI.Consumer.Domain.Entities.Validation;
using EventTriangleAPI.Shared.Application.Enums;
using FluentValidation;
using Uuids;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class CreditCardEntity
{
    public Guid Id { get; private set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();
    
    public Guid UserId { get; private set; }

    public string HolderName { get; private set; }

    public string CardNumber { get; private set; }
    
    public string Cvv { get; private set; }
    
    public PaymentNetwork PaymentNetwork { get; set; }

    public CreditCardEntity(Guid userId, string holderName, string cardNumber, string cvv, PaymentNetwork paymentNetwork)
    {
        UserId = userId;
        HolderName = holderName;
        CardNumber = cardNumber;
        Cvv = cvv;
        PaymentNetwork = paymentNetwork;
        
        new CreditCardEntityValidator().ValidateAndThrow(this);
    }
}
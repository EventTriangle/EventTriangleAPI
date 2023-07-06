using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.Application.Enums;
using EventTriangleAPI.Shared.Application.Enums.Events;
using FluentValidation;
using Uuids;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class CreditCardEventEntity
{
    public Guid Id { get; private set; } = Uuid.NewMySqlOptimized().ToGuidByteLayout();

    public Guid CardId { get; private set; }
    
    public Guid UserId { get; private set; }

    public string HolderName { get; private set; }
    
    public string CardNumber { get; private set; }
    
    public string Cvv { get; set; }
    
    public PaymentNetwork PaymentNetwork { get; private set; }
    
    public CardEventType CardEventType { get; private set; }

    public CreditCardEventEntity(
        Guid cardId,
        Guid userId,
        string holderName,
        string cardNumber,
        string cvv,
        PaymentNetwork paymentNetwork, 
        CardEventType cardEventType)
    {
        CardId = cardId;
        UserId = userId;
        HolderName = holderName;
        CardNumber = cardNumber;
        Cvv = cvv;
        PaymentNetwork = paymentNetwork;
        CardEventType = cardEventType;
        
        new CreditCardEventEntityValidator().ValidateAndThrow(this);
    }
}
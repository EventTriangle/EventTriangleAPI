using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class TransactionCardToUserCreatedEvent
{
    public Guid Id { get; private set; }
    
    public Guid CreditCardId { get; private set; }
    
    public string ToUserId { get; private set; }
    
    public decimal Amount { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public TransactionCardToUserCreatedEvent(Guid creditCardId, string toUserId, decimal amount)
    {
        Id = Guid.NewGuid();
        CreditCardId = creditCardId;
        ToUserId = toUserId;
        Amount = amount;
        CreatedAt = DateTime.UtcNow;
        
        new TransactionCardToUserCreatedEventValidator().ValidateAndThrow(this);
    }

    public TransactionCardToUserCreatedEventMessage CreateEventMessage()
    {
        return new TransactionCardToUserCreatedEventMessage(Id, CreditCardId, ToUserId, Amount, CreatedAt);
    }
}
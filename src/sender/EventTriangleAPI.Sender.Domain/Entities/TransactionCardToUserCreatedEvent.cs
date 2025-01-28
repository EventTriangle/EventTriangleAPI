using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class TransactionCardToUserCreatedEvent
{
    public Guid Id { get; private set; }
    
    public string RequesterId { get; private set; }
    
    public Guid CreditCardId { get; private set; }
    
    public decimal Amount { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    private static readonly TransactionCardToUserCreatedEventValidator Validator = new(); 

    public TransactionCardToUserCreatedEvent(string requesterId, Guid creditCardId, decimal amount)
    {
        Id = Guid.NewGuid();
        RequesterId = requesterId;
        CreditCardId = creditCardId;
        Amount = amount;
        CreatedAt = DateTime.UtcNow;
        
        Validator.ValidateAndThrow(this);
    }

    public TransactionCardToUserCreatedEventMessage CreateEventMessage()
    {
        return new TransactionCardToUserCreatedEventMessage(Id, RequesterId, CreditCardId, Amount, CreatedAt);
    }
}
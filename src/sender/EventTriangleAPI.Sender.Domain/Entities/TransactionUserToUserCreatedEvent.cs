using EventTriangleAPI.Sender.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Messages;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities;

public class TransactionUserToUserCreatedEvent
{
    public Guid Id { get; private set; }
    
    public string FromUserId { get; private set; }
    
    public string ToUserId { get; private set; }
    
    public decimal Amount { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public TransactionUserToUserCreatedEvent(string fromUserId, string toUserId, decimal amount)
    {
        Id = Guid.NewGuid();
        FromUserId = fromUserId;
        ToUserId = toUserId;
        Amount = amount;
        CreatedAt = DateTime.UtcNow;
        
        new TransactionUserToUserCreatedEventValidator().ValidateAndThrow(this);
    }

    public TransactionUserToUserCreatedEventMessage CreateEventMessage()
    {
        return new TransactionUserToUserCreatedEventMessage(Id, FromUserId, ToUserId, Amount, CreatedAt);
    }
}
using EventTriangleAPI.Sender.Domain.Entities;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public static class TransactionCardToUserCreatedEventHelper
{
    private const decimal Amount = 300;
    
    public static TransactionCardToUserCreatedEvent CreateSuccess()
    {
        return new TransactionCardToUserCreatedEvent(
            Guid.NewGuid(),
            Guid.NewGuid().ToString(),
            Amount);
    }
    
    public static TransactionCardToUserCreatedEvent CreateWithCreditCardId(Guid creditCardId)
    {
        return new TransactionCardToUserCreatedEvent(
            creditCardId,
            Guid.NewGuid().ToString(),
            Amount);
    }
    
    public static TransactionCardToUserCreatedEvent CreateWithToUserId(string toUserId)
    {
        return new TransactionCardToUserCreatedEvent(
            Guid.NewGuid(),
            toUserId,
            Amount);
    }
    
    public static TransactionCardToUserCreatedEvent CreateWithAmount(decimal amount)
    {
        return new TransactionCardToUserCreatedEvent(
            Guid.NewGuid(),
            Guid.NewGuid().ToString(),
            amount);
    }
}
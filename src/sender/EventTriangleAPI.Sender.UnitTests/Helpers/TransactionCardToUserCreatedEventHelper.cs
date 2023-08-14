using EventTriangleAPI.Sender.Domain.Entities;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public static class TransactionCardToUserCreatedEventHelper
{
    private const decimal Amount = 300;
    
    public static TransactionCardToUserCreatedEvent CreateSuccess()
    {
        return new TransactionCardToUserCreatedEvent(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            Amount);
    }
    
    public static TransactionCardToUserCreatedEvent CreateWithCreditCardId(Guid creditCardId)
    {
        return new TransactionCardToUserCreatedEvent(
            Guid.NewGuid().ToString(),
            creditCardId,
            Amount);
    }
    
    public static TransactionCardToUserCreatedEvent CreateWithToUserId(string toUserId)
    {
        return new TransactionCardToUserCreatedEvent(
            toUserId,
            Guid.NewGuid(),
            Amount);
    }
    
    public static TransactionCardToUserCreatedEvent CreateWithAmount(decimal amount)
    {
        return new TransactionCardToUserCreatedEvent(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            amount);
    }
}
using EventTriangleAPI.Sender.Domain.Entities;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public static class TransactionUserToUserCreatedEventHelper
{
    private const decimal Amount = 300;
    
    public static TransactionUserToUserCreatedEvent CreateSuccess()
    {
        return new TransactionUserToUserCreatedEvent(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Amount);
    }
    
    public static TransactionUserToUserCreatedEvent CreateWithFromUserId(string fromUserId)
    {
        return new TransactionUserToUserCreatedEvent(
            fromUserId,
            Guid.NewGuid().ToString(),
            Amount);
    }
    
    public static TransactionUserToUserCreatedEvent CreateWithToUserId(string toUserId)
    {
        return new TransactionUserToUserCreatedEvent(
            Guid.NewGuid().ToString(),
            toUserId,
            Amount);
    }
    
    public static TransactionUserToUserCreatedEvent CreateWithAmount(decimal amount)
    {
        return new TransactionUserToUserCreatedEvent(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            amount);
    }
}
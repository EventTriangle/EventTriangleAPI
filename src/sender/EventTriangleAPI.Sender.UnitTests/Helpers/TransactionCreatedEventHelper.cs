using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Shared.Application.Enums;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public static class TransactionCreatedEventHelper
{
    private const decimal Amount = 300;
    
    public static TransactionCreatedEvent CreateSuccess()
    {
        return new TransactionCreatedEvent(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Amount,
            TransactionType.FromUserToUser);
    }
    
    public static TransactionCreatedEvent CreateWithFrom(string from)
    {
        return new TransactionCreatedEvent(
            from,
            Guid.NewGuid().ToString(),
            Amount,
            TransactionType.FromUserToUser);
    }
    
    public static TransactionCreatedEvent CreateWithTo(string to)
    {
        return new TransactionCreatedEvent(
            Guid.NewGuid().ToString(),
            to,
            Amount,
            TransactionType.FromUserToUser);
    }
    
    public static TransactionCreatedEvent CreateWithAmount(decimal amount)
    {
        return new TransactionCreatedEvent(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            amount,
            TransactionType.FromUserToUser);
    }
}
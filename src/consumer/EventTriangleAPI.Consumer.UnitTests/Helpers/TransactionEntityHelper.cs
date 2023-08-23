using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.UnitTests.Helpers;

public class TransactionEntityHelper
{
    private const decimal Amount = 300;
    
    public static TransactionEntity CreateSuccess()
    {
        return new TransactionEntity(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Amount,
            TransactionType.FromUserToUser,
            DateTime.UtcNow);
    }
    
    public static TransactionEntity CreateWithFromUserId(string fromUserId)
    {
        return new TransactionEntity(
            fromUserId,
            Guid.NewGuid().ToString(),
            Amount,
            TransactionType.FromUserToUser,
            DateTime.UtcNow);
    }
    
    public static TransactionEntity CreateWithToUserId(string toUserId)
    {
        return new TransactionEntity(
            Guid.NewGuid().ToString(),
            toUserId,
            Amount,
            TransactionType.FromUserToUser,
            DateTime.UtcNow);
    }
    
    public static TransactionEntity CreateWithAmount(decimal amount)
    {
        return new TransactionEntity(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            amount,
            TransactionType.FromUserToUser,
            DateTime.UtcNow);
    }
}
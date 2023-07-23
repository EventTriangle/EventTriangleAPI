using EventTriangleAPI.Shared.Domain.Entities;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public static class TransactionRollBackedEventHelper
{
    public static TransactionRollBackedEvent CreateSuccess()
    {
        return new TransactionRollBackedEvent(Guid.NewGuid());
    }
    
    public static TransactionRollBackedEvent CreateWithTransactionId(Guid transactionId)
    {
        return new TransactionRollBackedEvent(transactionId);
    }
}
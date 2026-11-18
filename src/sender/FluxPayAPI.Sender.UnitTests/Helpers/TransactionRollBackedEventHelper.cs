using FluxPayAPI.Sender.Domain.Entities;

namespace FluxPayAPI.Sender.UnitTests.Helpers;

public static class TransactionRollBackedEventHelper
{
    public static TransactionRollBackedEvent CreateSuccess()
    {
        return new TransactionRollBackedEvent(Guid.NewGuid().ToString(),Guid.NewGuid());
    }
    
    public static TransactionRollBackedEvent CreateWithTransactionId(Guid transactionId)
    {
        return new TransactionRollBackedEvent(Guid.NewGuid().ToString(), transactionId);
    }
}
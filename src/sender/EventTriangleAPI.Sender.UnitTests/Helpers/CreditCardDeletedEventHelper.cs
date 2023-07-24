using EventTriangleAPI.Shared.Domain.Events;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public static class CreditCardDeletedEventHelper
{
    public static CreditCardDeletedEvent CreateSuccess()
    {
        return new CreditCardDeletedEvent(Guid.NewGuid().ToString(), Guid.NewGuid());
    }

    public static CreditCardDeletedEvent CreateWithUserId(string userId)
    {
        return new CreditCardDeletedEvent(userId, Guid.NewGuid());

    }
    
    public static CreditCardDeletedEvent CreateWithCardId(Guid cardId)
    {
        return new CreditCardDeletedEvent(Guid.NewGuid().ToString(), cardId);

    }
}
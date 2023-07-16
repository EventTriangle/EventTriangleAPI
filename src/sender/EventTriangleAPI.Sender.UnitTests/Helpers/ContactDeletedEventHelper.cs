using EventTriangleAPI.Sender.Domain.Entities;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public static class ContactDeletedEventHelper
{
    public static ContactDeletedEvent CreateSuccess()
    {
        return new ContactDeletedEvent(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
    }
    
    public static ContactDeletedEvent CreateWithUserId(string userId)
    {
        return new ContactDeletedEvent(userId, Guid.NewGuid().ToString());
    }
    
    public static ContactDeletedEvent CreateWithContactId(string contactId)
    {
        return new ContactDeletedEvent(Guid.NewGuid().ToString(), contactId);
    }
}
using EventTriangleAPI.Shared.Domain.Entities;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public static class ContactCreatedEventHelper
{
    public static ContactCreatedEvent CreateSuccess()
    {
        return new ContactCreatedEvent(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
    }
    
    public static ContactCreatedEvent CreateWithUserId(string userId)
    {
        return new ContactCreatedEvent(userId, Guid.NewGuid().ToString());
    }
    
    public static ContactCreatedEvent CreateWithContactId(string contactId)
    {
        return new ContactCreatedEvent(Guid.NewGuid().ToString(), contactId);
    }
}
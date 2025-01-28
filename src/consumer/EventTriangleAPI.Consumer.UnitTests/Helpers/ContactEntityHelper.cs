using EventTriangleAPI.Consumer.Domain.Entities;

namespace EventTriangleAPI.Consumer.UnitTests.Helpers;

public static class ContactEntityHelper
{
    public static ContactEntity CreateSuccess()
    {
        return new ContactEntity(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
    }
    
    public static ContactEntity CreateWithUserId(string userId)
    {
        return new ContactEntity(userId, Guid.NewGuid().ToString());
    }
    
    public static ContactEntity CreateWithContactId(string contactId)
    {
        return new ContactEntity(Guid.NewGuid().ToString(), contactId);
    }
}
using EventTriangleAPI.Sender.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Sender.UnitTests.Domain;

public class ContactDeletedEventTests
{
    [Fact]
    public void TestSuccess()
    {
        var createContactDeletedEvent = ContactDeletedEventHelper.CreateSuccess;

        createContactDeletedEvent.Should().NotThrow();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowUserId(string userId)
    {
        var createContactDeletedEvent = () => ContactCreatedEventHelper.CreateWithUserId(userId);

        createContactDeletedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowContactId(string contactId)
    {
        var createContactDeletedEvent = () => ContactCreatedEventHelper.CreateWithContactId(contactId);

        createContactDeletedEvent.Should().ThrowExactly<ValidationException>();
    }
}
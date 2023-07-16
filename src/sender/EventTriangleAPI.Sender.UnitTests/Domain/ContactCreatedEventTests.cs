using EventTriangleAPI.Sender.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Sender.UnitTests.Domain;

public class ContactCreatedEventTests
{
    [Fact]
    public void TestSuccess()
    {
        var createContactCreatedEvent = ContactCreatedEventHelper.CreateSuccess;

        createContactCreatedEvent.Should().NotThrow();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowUserId(string userId)
    {
        var createContactCreatedEvent = () => ContactCreatedEventHelper.CreateWithUserId(userId);

        createContactCreatedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowContactId(string contactId)
    {
        var createContactCreatedEvent = () => ContactCreatedEventHelper.CreateWithContactId(contactId);

        createContactCreatedEvent.Should().ThrowExactly<ValidationException>();
    }
}
using EventTriangleAPI.Consumer.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Consumer.UnitTests.Domain;

public class ContactEntityTests
{
    [Fact]
    public void TestSuccess()
    {
        var result = ContactEntityHelper.CreateSuccess;

        result.Should().NotThrow();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowUserId(string userId)
    {
        var result = () => ContactEntityHelper.CreateWithUserId(userId);

        result.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowContactId(string contactId)
    {
        var result = () => ContactEntityHelper.CreateWithContactId(contactId);

        result.Should().ThrowExactly<ValidationException>();
    }
}
using EventTriangleAPI.Sender.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Sender.UnitTests.Domain;

public class UserCreatedEventTests
{
    [Fact]
    public void TestSuccess()
    {
        var createUserCreatedEvent = UserCreatedEventHelper.CreateSuccess;

        createUserCreatedEvent.Should().NotThrow();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowWithUserId(string userId)
    {
        var createUserCreatedEvent = () => UserCreatedEventHelper.CreateWithUserId(userId);

        createUserCreatedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("1234")]
    public void TestThrowWithEmail(string email)
    {
        var createUserCreatedEvent = () => UserCreatedEventHelper.CreateWithEmail(email);

        createUserCreatedEvent.Should().ThrowExactly<ValidationException>();
    }
}
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
}
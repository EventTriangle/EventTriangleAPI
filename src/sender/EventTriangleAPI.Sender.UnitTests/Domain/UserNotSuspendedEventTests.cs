using EventTriangleAPI.Sender.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Sender.UnitTests.Domain;

public class UserNotSuspendedEventTests
{
    [Fact]
    public void TestSuccess()
    {
        var createUserNotSuspendedEvent = UserNotSuspendedEventHelper.CreateSuccess;

        createUserNotSuspendedEvent.Should().NotThrow();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowWithUserId(string userId)
    {
        var createUserNotSuspendedEvent = () => UserNotSuspendedEventHelper.CreateWithUserId(userId);

        createUserNotSuspendedEvent.Should().ThrowExactly<ValidationException>();
    }
}
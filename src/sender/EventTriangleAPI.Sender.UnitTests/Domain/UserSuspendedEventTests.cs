using EventTriangleAPI.Sender.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Sender.UnitTests.Domain;

public class UserSuspendedEventTests
{
    [Fact]
    public void TestSuccess()
    {
        var createUserSuspendedEvent = UserSuspendedEventHelper.CreateSuccess;

        createUserSuspendedEvent.Should().NotThrow();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowWithUserId(string userId)
    {
        var createUserSuspendedEvent = () => UserSuspendedEventHelper.CreateWithUserId(userId);

        createUserSuspendedEvent.Should().ThrowExactly<ValidationException>();
    }
}
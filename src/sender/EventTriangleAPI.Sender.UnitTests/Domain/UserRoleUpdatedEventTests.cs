using EventTriangleAPI.Sender.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Sender.UnitTests.Domain;

public class UserRoleUpdatedEventTests
{
    [Fact]
    public void TestSuccess()
    {
        var createUserRoleUpdatedEvent = UserRoleUpdatedEventHelper.CreateSuccess;

        createUserRoleUpdatedEvent.Should().NotThrow();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowUserId(string userId)
    {
        var createUserRoleUpdatedEvent = () => UserRoleUpdatedEventHelper.CreateWithUserId(userId);

        createUserRoleUpdatedEvent.Should().ThrowExactly<ValidationException>();
    }
}
using EventTriangleAPI.Consumer.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Consumer.UnitTests.Domain;

public class UserEntityTests
{
    [Fact]
    public void TestSuccess()
    {
        var result = UserEntityHelper.CreateSuccess;

        result.Should().NotThrow();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowWithUserId(string userId)
    {
        var result = () => UserEntityHelper.CreateWithUserId(userId);

        result.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("1234")]
    public void TestThrowWithEmail(string email)
    {
        var result = () => UserEntityHelper.CreateWithEmail(email);

        result.Should().ThrowExactly<ValidationException>();
    }
}
using EventTriangleAPI.Consumer.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Consumer.UnitTests.Domain;

public class TransactionEntityTests
{
    [Fact]
    public void TestSuccess()
    {
        var result = TransactionEntityHelper.CreateSuccess;

        result.Should().NotThrow();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowFromUserId(string fromUserId)
    {
        var result = () => TransactionEntityHelper.CreateWithFromUserId(fromUserId);

        result.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowTo(string toUserId)
    {
        var result = () => TransactionEntityHelper.CreateWithToUserId(toUserId);

        result.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData(-300)]
    public void TestThrowAmount(decimal amount)
    {
        var result = () => TransactionEntityHelper.CreateWithAmount(amount);

        result.Should().ThrowExactly<ValidationException>();
    }
}
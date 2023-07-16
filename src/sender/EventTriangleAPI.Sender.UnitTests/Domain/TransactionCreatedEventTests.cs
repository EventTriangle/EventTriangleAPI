using EventTriangleAPI.Sender.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Sender.UnitTests.Domain;

public class TransactionCreatedEventTests
{
    [Fact]
    public void TestSuccess()
    {
        var createTransactionCreatedEvent = TransactionCreatedEventHelper.CreateSuccess;

        createTransactionCreatedEvent.Should().NotThrow();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowFrom(string from)
    {
        var createTransactionCreatedEvent = () => TransactionCreatedEventHelper.CreateWithFrom(from);

        createTransactionCreatedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowTo(string to)
    {
        var createTransactionCreatedEvent = () => TransactionCreatedEventHelper.CreateWithTo(to);

        createTransactionCreatedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData(-300)]
    public void TestThrowAmount(decimal amount)
    {
        var createTransactionCreatedEvent = () => TransactionCreatedEventHelper.CreateWithAmount(amount);

        createTransactionCreatedEvent.Should().ThrowExactly<ValidationException>();
    }
}
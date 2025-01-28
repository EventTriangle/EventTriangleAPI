using EventTriangleAPI.Sender.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Sender.UnitTests.Domain;

public class TransactionUserToUserCreatedEventTests
{
    [Fact]
    public void TestSuccess()
    {
        var createTransactionUserToUserCreatedEvent = TransactionUserToUserCreatedEventHelper.CreateSuccess;

        createTransactionUserToUserCreatedEvent.Should().NotThrow();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowFromUserId(string fromUserId)
    {
        var createTransactionUserToUserCreatedEvent = () => TransactionUserToUserCreatedEventHelper.CreateWithFromUserId(fromUserId);

        createTransactionUserToUserCreatedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowTo(string toUserId)
    {
        var createTransactionUserToUserCreatedEvent = () => TransactionUserToUserCreatedEventHelper.CreateWithToUserId(toUserId);

        createTransactionUserToUserCreatedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData(-300)]
    public void TestThrowAmount(decimal amount)
    {
        var createTransactionUserToUserCreatedEvent = () => TransactionUserToUserCreatedEventHelper.CreateWithAmount(amount);

        createTransactionUserToUserCreatedEvent.Should().ThrowExactly<ValidationException>();
    }
}
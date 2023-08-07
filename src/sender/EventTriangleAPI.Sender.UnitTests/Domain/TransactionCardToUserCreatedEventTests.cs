using EventTriangleAPI.Sender.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Sender.UnitTests.Domain;

public class TransactionCardToUserCreatedEventTests
{
    [Fact]
    public void TestSuccess()
    {
        var createTransactionCardToUserCreatedEvent = TransactionCardToUserCreatedEventHelper.CreateSuccess;

        createTransactionCardToUserCreatedEvent.Should().NotThrow();
    }
    
    [Fact]
    public void TestThrowCreditCardId()
    {
        var createTransactionCardToUserCreatedEvent = () => TransactionCardToUserCreatedEventHelper.CreateWithCreditCardId(Guid.Empty);

        createTransactionCardToUserCreatedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowTo(string toUserId)
    {
        var createTransactionCardToUserCreatedEvent = () => TransactionCardToUserCreatedEventHelper.CreateWithToUserId(toUserId);

        createTransactionCardToUserCreatedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData(-300)]
    public void TestThrowAmount(decimal amount)
    {
        var createTransactionCardToUserCreatedEvent = () => TransactionCardToUserCreatedEventHelper.CreateWithAmount(amount);

        createTransactionCardToUserCreatedEvent.Should().ThrowExactly<ValidationException>();
    }
}
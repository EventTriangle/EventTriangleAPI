using EventTriangleAPI.Sender.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Sender.UnitTests.Domain;

public class CreditCardAddedEventTests
{
    [Fact]
    public void TestSuccess()
    {
        var createCreditCardAddedEvent = CreditCardAddedEventHelper.CreateSuccess;

        createCreditCardAddedEvent.Should().NotThrow();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowUserId(string userId)
    {
        var createCreditCardAddedEvent = () => CreditCardAddedEventHelper.CreateWithUserId(userId);

        createCreditCardAddedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowHolderName(string holderName)
    {
        var createCreditCardAddedEvent = () => CreditCardAddedEventHelper.CreateWithHolderName(holderName);

        createCreditCardAddedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("1234567890")]
    [InlineData("12345678901234567890")]
    public void TestThrowCreditNumber(string creditNumber)
    {
        var createCreditCardAddedEvent = () => CreditCardAddedEventHelper.CreateWithCardNumber(creditNumber);

        createCreditCardAddedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("12")]
    [InlineData("1234")]
    public void TestThrowCvv(string cvv)
    {
        var createCreditCardAddedEvent = () => CreditCardAddedEventHelper.CreateWithCvv(cvv);

        createCreditCardAddedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("1/2")]
    [InlineData("123/123")]
    [InlineData("123/123/43")]
    public void TestThrowExpiration(string expiration)
    {
        var createCreditCardAddedEvent = () => CreditCardAddedEventHelper.CreateWithExpiration(expiration);

        createCreditCardAddedEvent.Should().ThrowExactly<ValidationException>();
    }
}
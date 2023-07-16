using EventTriangleAPI.Sender.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Sender.UnitTests.Domain;

public class CreditCardChangedEventTests
{
    [Fact]
    public void TestSuccess()
    {
        var createCreditCardChangedEvent = CreditCardChangedEventHelper.CreateSuccess;

        createCreditCardChangedEvent.Should().NotThrow();
    }

    [Fact]
    public void TestThrowCardId()
    {
        var createCreditCardChangedEvent = () => CreditCardChangedEventHelper.CreateWithCardId(Guid.Empty);

        createCreditCardChangedEvent.Should().ThrowExactly<ValidationException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowUserId(string userId)
    {
        var createCreditCardChangedEvent = () => CreditCardChangedEventHelper.CreateWithUserId(userId);

        createCreditCardChangedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowHolderName(string holderName)
    {
        var createCreditCardChangedEvent = () => CreditCardChangedEventHelper.CreateWithHolderName(holderName);

        createCreditCardChangedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("1234567890")]
    [InlineData("12345678901234567890")]
    public void TestThrowCreditNumber(string creditNumber)
    {
        var createCreditCardChangedEvent = () => CreditCardChangedEventHelper.CreateWithCardNumber(creditNumber);

        createCreditCardChangedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("12")]
    [InlineData("1234")]
    public void TestThrowCvv(string cvv)
    {
        var createCreditCardChangedEvent = () => CreditCardChangedEventHelper.CreateWithCvv(cvv);

        createCreditCardChangedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("1/2")]
    [InlineData("123/123")]
    [InlineData("123/123/43")]
    public void TestThrowExpiration(string expiration)
    {
        var createCreditCardChangedEvent = () => CreditCardChangedEventHelper.CreateWithExpiration(expiration);

        createCreditCardChangedEvent.Should().ThrowExactly<ValidationException>();
    }
}
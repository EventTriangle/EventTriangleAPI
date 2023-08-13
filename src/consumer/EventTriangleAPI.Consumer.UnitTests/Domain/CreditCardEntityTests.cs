using EventTriangleAPI.Consumer.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Consumer.UnitTests.Domain;

public class CreditCardEntityTests
{
    [Fact]
    public void TestSuccess()
    {
        var result = CreditCardEntityHelper.CreateSuccess;

        result.Should().NotThrow();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowUserId(string userId)
    {
        var result = () => CreditCardEntityHelper.CreateWithUserId(userId);

        result.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowHolderName(string holderName)
    {
        var result = () => CreditCardEntityHelper.CreateWithHolderName(holderName);

        result.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("1234567890")]
    [InlineData("12345678901234567890")]
    public void TestThrowCreditNumber(string creditNumber)
    {
        var result = () => CreditCardEntityHelper.CreateWithCardNumber(creditNumber);

        result.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("12")]
    [InlineData("1234")]
    public void TestThrowCvv(string cvv)
    {
        var result = () => CreditCardEntityHelper.CreateWithCvv(cvv);

        result.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("1/2")]
    [InlineData("123/123")]
    [InlineData("123/123/43")]
    public void TestThrowExpiration(string expiration)
    {
        var result = () => CreditCardEntityHelper.CreateWithExpiration(expiration);

        result.Should().ThrowExactly<ValidationException>();
    }
}
using EventTriangleAPI.Sender.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Sender.UnitTests.Domain;

public class CreditCardDeletedEventTests
{
    [Fact]
    public void TestSuccess()
    {
        var createCreditCardDeletedEvent = CreditCardDeletedEventHelper.CreateSuccess;

        createCreditCardDeletedEvent.Should().NotThrow();
    }

    [Fact]
    public void TestThrowCardId()
    {
        var createCreditCardDeletedEvent = () => CreditCardDeletedEventHelper.CreateWithCardId(Guid.Empty);

        createCreditCardDeletedEvent.Should().ThrowExactly<ValidationException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowUserId(string userId)
    {
        var createCreditCardDeletedEvent = () => CreditCardDeletedEventHelper.CreateWithUserId(userId);

        createCreditCardDeletedEvent.Should().ThrowExactly<ValidationException>();
    }
}
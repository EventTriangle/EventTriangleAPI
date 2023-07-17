using EventTriangleAPI.Sender.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Sender.UnitTests.Domain;

public class TransactionRollBackedEventTests
{
    [Fact]
    public void TestSuccess()
    {
        var createTransactionRollBackedEvent = TransactionRollBackedEventHelper.CreateSuccess;

        createTransactionRollBackedEvent.Should().NotThrow();
    }
    
    [Fact]
    public void TestThrowTransactionId()
    {
        var createTransactionRollBackedEvent = () => TransactionRollBackedEventHelper.CreateWithTransactionId(Guid.Empty);

        createTransactionRollBackedEvent.Should().ThrowExactly<ValidationException>();
    }
}
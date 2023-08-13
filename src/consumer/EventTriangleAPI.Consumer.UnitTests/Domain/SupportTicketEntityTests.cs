using EventTriangleAPI.Consumer.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Consumer.UnitTests.Domain;

public class SupportTicketEntityTests
{
    [Fact]
    public void TestSuccess()
    {
        var result = SupportTicketEntityHelper.CreateSuccess;

        result.Should().NotThrow();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowUserId(string userId)
    {
        var result = () => SupportTicketEntityHelper.CreateWithUserId(userId);

        result.Should().ThrowExactly<ValidationException>();
    }
    
    [Fact]
    public void TestThrowWalletId()
    {
        var result = () => SupportTicketEntityHelper.CreateWithWalletId(Guid.Empty);

        result.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowTicketReason(string ticketReason)
    {
        var result = () => SupportTicketEntityHelper.CreateWithTicketReason(ticketReason);

        result.Should().ThrowExactly<ValidationException>();
    }
    
    [Fact]
    public void TestThrowTicketReasonOverflow()
    {
        var result = () => SupportTicketEntityHelper.CreateWithTicketReason(new string('a', 301));

        result.Should().ThrowExactly<ValidationException>();
    }

    [Fact]
    public void TestThrowTicketJustificationOverflow()
    {
        var result = () => SupportTicketEntityHelper.CreateWithTicketJustification(new string('a', 301));

        result.Should().ThrowExactly<ValidationException>();
    }
}
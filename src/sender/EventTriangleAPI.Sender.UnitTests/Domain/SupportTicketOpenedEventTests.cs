using EventTriangleAPI.Sender.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Sender.UnitTests.Domain;

public class SupportTicketOpenedEventTests
{
    [Fact]
    public void TestSuccess()
    {
        var createSupportTicketOpenedEvent = SupportTicketOpenedEventHelper.CreateSuccess;

        createSupportTicketOpenedEvent.Should().NotThrow();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowUserId(string userId)
    {
        var createSupportTicketOpenedEvent = () => SupportTicketOpenedEventHelper.CreateWithUserId(userId);

        createSupportTicketOpenedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Fact]
    public void TestThrowWalletId()
    {
        var createSupportTicketOpenedEvent = () => SupportTicketOpenedEventHelper.CreateWithWalletId(Guid.Empty);

        createSupportTicketOpenedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowTicketReason(string ticketReason)
    {
        var createSupportTicketOpenedEvent = () => SupportTicketOpenedEventHelper.CreateWithTicketReason(ticketReason);

        createSupportTicketOpenedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Fact]
    public void TestThrowTicketReasonOverflow()
    {
        var createSupportTicketOpenedEvent = () => SupportTicketOpenedEventHelper.CreateWithTicketReason(new string('a', 301));

        createSupportTicketOpenedEvent.Should().ThrowExactly<ValidationException>();
    }
}

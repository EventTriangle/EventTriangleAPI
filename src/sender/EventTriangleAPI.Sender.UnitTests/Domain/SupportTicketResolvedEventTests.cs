using EventTriangleAPI.Sender.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace EventTriangleAPI.Sender.UnitTests.Domain;

public class SupportTicketResolvedEventTests
{
    [Fact]
    public void TestSuccess()
    {
        var createSupportTicketResolvedEvent = SupportTicketResolvedEventHelper.CreateSuccess;

        createSupportTicketResolvedEvent.Should().NotThrow();
    }
    
    [Fact]
    public void TestThrowWithTicketId()
    {
        var createSupportTicketResolvedEvent = () => SupportTicketResolvedEventHelper.CreateWithTicketId(Guid.Empty);

        createSupportTicketResolvedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestThrowWithTicketJustification(string ticketJustification)
    {
        var createSupportTicketResolvedEvent = () => SupportTicketResolvedEventHelper.CreateWithTicketJustification(ticketJustification);

        createSupportTicketResolvedEvent.Should().ThrowExactly<ValidationException>();
    }
    
    [Fact]
    public void TestThrowWithTicketJustificationOverflow()
    {
        var createSupportTicketResolvedEvent = () => 
            SupportTicketResolvedEventHelper.CreateWithTicketJustification(new string('a', 301));

        createSupportTicketResolvedEvent.Should().ThrowExactly<ValidationException>();
    }
}
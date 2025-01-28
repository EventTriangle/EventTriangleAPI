using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class OpenSupportTicketCommandHandlerTest(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task Test()
    {
        var command = new OpenSupportTicketCommand(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid().ToString());

        var result = await Fixture.OpenSupportTicketCommandHandler.HandleAsync(command);

        var supportTicketOpenedEvent =
            await Fixture.DatabaseContextFixture.SupportTicketOpenedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        supportTicketOpenedEvent.Should().NotBeNull();
    }
}

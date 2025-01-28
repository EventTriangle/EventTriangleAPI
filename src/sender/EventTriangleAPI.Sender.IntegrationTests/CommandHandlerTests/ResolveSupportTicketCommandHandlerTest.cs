using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class ResolveSupportTicketCommandHandlerTest(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task Test()
    {
        var command = new ResolveSupportTicketCommand(Guid.NewGuid().ToString(), Guid.NewGuid(), Guid.NewGuid().ToString());

        var result = await Fixture.ResolveSupportTicketCommandHandler.HandleAsync(command);

        var supportTicketResolvedEvent =
            await Fixture.DatabaseContextFixture.SupportTicketResolvedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        supportTicketResolvedEvent.Should().NotBeNull();
    }
}

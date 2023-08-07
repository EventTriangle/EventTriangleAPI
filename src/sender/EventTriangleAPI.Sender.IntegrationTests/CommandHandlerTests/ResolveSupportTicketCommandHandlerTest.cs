using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class ResolveSupportTicketCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var command = new ResolveSupportTicketCommand(Guid.NewGuid(), Guid.NewGuid().ToString());
        
        var result = await ResolveSupportTicketCommandHandler.HandleAsync(command);

        var supportTicketResolvedEvent = 
            await DatabaseContextFixture.SupportTicketResolvedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        supportTicketResolvedEvent.Should().NotBeNull();
    }
}
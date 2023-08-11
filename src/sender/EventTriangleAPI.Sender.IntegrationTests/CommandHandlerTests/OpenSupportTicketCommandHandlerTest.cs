using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class OpenSupportTicketCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var command = new OpenSupportTicketCommand(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            Guid.NewGuid().ToString());
        
        var result = await OpenSupportTicketCommandHandler.HandleAsync(command);

        var supportTicketOpenedEvent = 
            await DatabaseContextFixture.SupportTicketOpenedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        supportTicketOpenedEvent.Should().NotBeNull();
    }
}
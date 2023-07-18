using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.DTO.Commands;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class OpenSupportTicketCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var body = new OpenSupportTicketBody(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            Guid.NewGuid().ToString());
        var command = new Command<OpenSupportTicketBody>(body);
        
        var result = await OpenSupportTicketCommandHandler.HandleAsync(command);

        var addContactEvent = 
            await DatabaseContextFixture.SupportTicketOpenedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        addContactEvent.Should().NotBeNull();
    }
}
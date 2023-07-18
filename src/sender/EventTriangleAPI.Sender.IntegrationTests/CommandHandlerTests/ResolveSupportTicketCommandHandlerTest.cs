using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.DTO.Commands;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class ResolveSupportTicketCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var body = new ResolveSupportTicketBody(Guid.NewGuid(), Guid.NewGuid().ToString());
        var command = new Command<ResolveSupportTicketBody>(body);
        
        var result = await ResolveSupportTicketCommandHandler.HandleAsync(command);

        var addContactEvent = 
            await DatabaseContextFixture.SupportTicketResolvedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        addContactEvent.Should().NotBeNull();
    }
}
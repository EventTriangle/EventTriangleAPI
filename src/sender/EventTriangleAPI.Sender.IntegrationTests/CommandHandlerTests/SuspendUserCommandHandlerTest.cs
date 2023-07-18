using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.DTO.Commands;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class SuspendUserCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var body = new SuspendUserBody(Guid.NewGuid().ToString());
        var command = new Command<SuspendUserBody>(body);
        
        var result = await SuspendUserCommandHandler.HandleAsync(command);

        var addContactEvent = 
            await DatabaseContextFixture.UserSuspendedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        addContactEvent.Should().NotBeNull();
    }
}
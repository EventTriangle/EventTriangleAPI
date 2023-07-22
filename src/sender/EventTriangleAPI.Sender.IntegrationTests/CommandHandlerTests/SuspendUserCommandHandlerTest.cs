using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class SuspendUserCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var command = new SuspendUserCommand(Guid.NewGuid().ToString());
        
        var result = await SuspendUserCommandHandler.HandleAsync(command);

        var addContactEvent = 
            await DatabaseContextFixture.UserSuspendedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        addContactEvent.Should().NotBeNull();
    }
}
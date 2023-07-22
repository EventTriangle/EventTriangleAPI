using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class NotSuspendUserCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var command = new NotSuspendUserCommand(Guid.NewGuid().ToString());
        
        var result = await NotSuspendUserCommandHandler.HandleAsync(command);

        var addContactEvent = 
            await DatabaseContextFixture.UserNotSuspendedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        addContactEvent.Should().NotBeNull();
    }
}
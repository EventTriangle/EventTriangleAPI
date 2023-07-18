using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.DTO.Commands;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class NotSuspendUserCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var body = new NotSuspendUserBody(Guid.NewGuid().ToString());
        var command = new Command<NotSuspendUserBody>(body);
        
        var result = await NotSuspendUserCommandHandler.HandleAsync(command);

        var addContactEvent = 
            await DatabaseContextFixture.UserNotSuspendedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        addContactEvent.Should().NotBeNull();
    }
}
using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class DeleteContactCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var command = new DeleteContactCommand(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        
        var result = await DeleteContactCommandHandler.HandleAsync(command);

        var addContactEvent = 
            await DatabaseContextFixture.ContactDeletedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        addContactEvent.Should().NotBeNull();
    }
}
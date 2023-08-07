using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class AddContactCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var command = new AddContactCommand(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        
        var result = await AddContactCommandHandler.HandleAsync(command);

        var contactAddedEvent = 
            await DatabaseContextFixture.ContactCreatedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        contactAddedEvent.Should().NotBeNull();
    }
}
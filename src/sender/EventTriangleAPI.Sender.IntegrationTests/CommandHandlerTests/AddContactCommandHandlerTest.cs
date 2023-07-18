using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.DTO.Commands;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class AddContactCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var body = new AddContactBody(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        var command = new Command<AddContactBody>(body);
        
        var result = await AddContactCommandHandler.HandleAsync(command);

        var addContactEvent = 
            await DatabaseContextFixture.ContactCreatedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        addContactEvent.Should().NotBeNull();
    }
}
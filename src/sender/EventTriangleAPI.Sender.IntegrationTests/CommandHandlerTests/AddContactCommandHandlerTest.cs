using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class AddContactCommandHandlerTest(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task Test()
    {
        var command = new AddContactCommand(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        var result = await Fixture.AddContactCommandHandler.HandleAsync(command);

        var contactAddedEvent =
            await Fixture.DatabaseContextFixture.ContactCreatedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        contactAddedEvent.Should().NotBeNull();
    }
}

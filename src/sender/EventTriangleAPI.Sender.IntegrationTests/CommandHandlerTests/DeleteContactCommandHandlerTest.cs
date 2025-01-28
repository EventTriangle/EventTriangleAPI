using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class DeleteContactCommandHandlerTest(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task Test()
    {
        var command = new DeleteContactCommand(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        var result = await Fixture.DeleteContactCommandHandler.HandleAsync(command);

        var contactDeletedEvent =
            await Fixture.DatabaseContextFixture.ContactDeletedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        contactDeletedEvent.Should().NotBeNull();
    }
}

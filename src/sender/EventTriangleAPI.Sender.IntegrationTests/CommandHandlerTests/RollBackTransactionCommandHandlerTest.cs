using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class RollBackTransactionCommandHandlerTest(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task Test()
    {
        var command = new RollBackTransactionCommand(Guid.NewGuid().ToString(), Guid.NewGuid());

        var result = await Fixture.RollBackTransactionCommandHandler.HandleAsync(command);

        var transactionRollBackedEvent =
            await Fixture.DatabaseContextFixture.TransactionRollBackedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        transactionRollBackedEvent.Should().NotBeNull();
    }
}

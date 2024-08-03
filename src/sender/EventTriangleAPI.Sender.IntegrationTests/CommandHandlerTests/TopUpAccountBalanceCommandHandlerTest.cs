using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class TopUpAccountBalanceCommandHandlerTest(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task Test()
    {
        var command = new TopUpAccountBalanceCommand(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            Amount: 300);

        var result = await Fixture.TopUpAccountBalanceCommandHandler.HandleAsync(command);

        var transactionCreatedEvent =
            await Fixture.DatabaseContextFixture.TransactionCardToUserCreatedEvents.FirstOrDefaultAsync(x =>
                x.Id == result.Response.Id);

        transactionCreatedEvent.Should().NotBeNull();
    }
}

using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class TopUpAccountBalanceCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var command = new TopUpAccountBalanceCommand(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            300);

        var result = await TopUpAccountBalanceCommandHandler.HandleAsync(command);

        var transactionCreatedEvent =
            await DatabaseContextFixture.TransactionCardToUserCreatedEvents.FirstOrDefaultAsync(x =>
                x.Id == result.Response.Id);

        transactionCreatedEvent.Should().NotBeNull();
    }
}
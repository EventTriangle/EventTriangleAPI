using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class CreateTransactionUserToUserCommandHandlerTest(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task Test()
    {
        var command = new CreateTransactionUserToUserCommand(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            Amount: 300);

        var result = await Fixture.CreateTransactionUserToUserCommandHandler.HandleAsync(command);

        var transactionCreatedEvent =
            await Fixture.DatabaseContextFixture.TransactionUserToUserCreatedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        transactionCreatedEvent.Should().NotBeNull();
    }
}

using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.DTO.Commands;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class RollBackTransactionCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var body = new RollBackTransactionBody(Guid.NewGuid());
        var command = new Command<RollBackTransactionBody>(body);
        
        var result = await RollBackTransactionCommandHandler.HandleAsync(command);

        var addContactEvent = 
            await DatabaseContextFixture.TransactionRollBackedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        addContactEvent.Should().NotBeNull();
    }
}
using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.Domain.Enums;
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
            Guid.NewGuid().ToString(),
            300,
            TransactionType.FromCardToUser);
        
        var result = await TopUpAccountBalanceCommandHandler.HandleAsync(command);

        var addContactEvent = 
            await DatabaseContextFixture.TransactionCreatedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        addContactEvent.Should().NotBeNull();
    }
}
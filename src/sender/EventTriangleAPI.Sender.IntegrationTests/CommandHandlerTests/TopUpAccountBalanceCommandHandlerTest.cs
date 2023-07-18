using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.Application.Enums;
using EventTriangleAPI.Shared.DTO.Commands;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class TopUpAccountBalanceCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var body = new TopUpAccountBalanceBody(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            300,
            TransactionType.FromCardToUser);
        var command = new Command<TopUpAccountBalanceBody>(body);
        
        var result = await TopUpAccountBalanceCommandHandler.HandleAsync(command);

        var addContactEvent = 
            await DatabaseContextFixture.TransactionCreatedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        addContactEvent.Should().NotBeNull();
    }
}
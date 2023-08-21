using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.OpenSupportTicketCommandHandlerTests;

public class OpenSupportTicketTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var alice = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserAliceCommand());

        var openSupportTicketCommand = new OpenSupportTicketCommand(
            alice.Response.Id,
            alice.Response.WalletId,
            "Please, can you rollback my transaction?");

        var openSupportTicketResult = await OpenSupportTicketCommandHandler.HandleAsync(openSupportTicketCommand);

        var isSupportTicketExists = await DatabaseContextFixture.SupportTicketEntities
            .AnyAsync(x => x.Id == openSupportTicketResult.Response.Id);

        isSupportTicketExists.Should().BeTrue();
    }
}
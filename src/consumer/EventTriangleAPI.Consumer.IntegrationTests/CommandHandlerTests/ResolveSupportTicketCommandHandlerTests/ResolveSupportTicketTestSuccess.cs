using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.ResolveSupportTicketCommandHandlerTests;

public class ResolveSupportTicketTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserAliceCommand());
        
        var openSupportTicketCommand = new OpenSupportTicketCommand(
            alice.Response.Id,
            alice.Response.WalletId,
            "Please, can you rollback my transaction?");
        
        var openSupportTicketResult = await OpenSupportTicketCommandHandler.HandleAsync(openSupportTicketCommand);

        var supportTicket = await DatabaseContextFixture.SupportTicketEntities
            .FirstOrDefaultAsync(x => x.Id == openSupportTicketResult.Response.Id);

        supportTicket.TicketStatus.Should().Be(TicketStatus.Open);
        
        var resolveSupportTicketCommand =  new ResolveSupportTicketCommand(
            dima.Response.Id, 
            openSupportTicketResult.Response.Id,
            "Transaction is rolled back");

        await ResolveSupportTicketCommandHandler.HandleAsync(resolveSupportTicketCommand);
        
        var supportTicketAfterResolving = await DatabaseContextFixture.SupportTicketEntities
            .FirstOrDefaultAsync(x => x.Id == openSupportTicketResult.Response.Id);

        supportTicketAfterResolving.TicketStatus.Should().Be(TicketStatus.Resolved);
    }
}
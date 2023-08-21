using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.ResolveSupportTicketCommandHandlerTests;

public class ResolveSupportTicketTestThrowConflict : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterIsNotAdmin()
    {
        var bob = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserBobCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserAliceCommand());
        
        var openSupportTicketCommand = new OpenSupportTicketCommand(
            alice.Response.Id,
            alice.Response.WalletId,
            "Please, can you rollback my transaction?");
        
        var openSupportTicketResult = await OpenSupportTicketCommandHandler.HandleAsync(openSupportTicketCommand);

        var resolveSupportTicketCommand =  new ResolveSupportTicketCommand(
            bob.Response.Id, 
            openSupportTicketResult.Response.Id,
            "Transaction is rolled back");

        var resolveSupportTicketResult = await ResolveSupportTicketCommandHandler.HandleAsync(resolveSupportTicketCommand);

        resolveSupportTicketResult.Error.Should().BeOfType<ConflictError>();
    }
}
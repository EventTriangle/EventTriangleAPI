using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.ResolveSupportTicketCommandHandlerTests;

public class ResolveSupportTicketTestThrowEntityNotFound : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var alice = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        
        var openSupportTicketCommand = new OpenSupportTicketCommand(
            alice.Response.Id,
            alice.Response.WalletId,
            "Please, can you rollback my transaction?");
        
        var openSupportTicketResult = await OpenSupportTicketCommandHandler.HandleAsync(openSupportTicketCommand);

        var resolveSupportTicketCommand =  new ResolveSupportTicketCommand(
            Guid.NewGuid().ToString(), 
            openSupportTicketResult.Response.Id,
            "Transaction is rolled back");

        await ResolveSupportTicketCommandHandler.HandleAsync(resolveSupportTicketCommand);
    }
    
    [Fact]
    public async Task TestSupportTicketNotFound()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        
        var resolveSupportTicketCommand =  new ResolveSupportTicketCommand(
            dima.Response.Id, 
            Guid.NewGuid(),
            "Transaction is rolled back");

        await ResolveSupportTicketCommandHandler.HandleAsync(resolveSupportTicketCommand);
    }
}
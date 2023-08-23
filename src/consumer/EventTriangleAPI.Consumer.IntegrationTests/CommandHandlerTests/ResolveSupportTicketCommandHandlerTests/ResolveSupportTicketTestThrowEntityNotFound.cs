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
        var bob = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());

        var addCreditCardForAliceCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(alice.Response.Id);

        var addCreditCardForAliceResult = await AddCreditCardCommandHandler.HandleAsync(addCreditCardForAliceCommand);

        var createTransactionCardToUserForDimaCommand = new CreateTransactionCardToUserCommand(
            addCreditCardForAliceResult.Response.Id,
            alice.Response.Id,
            300,
            DateTime.UtcNow);

        await CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserForDimaCommand);
        
        var createTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            alice.Response.Id,
            bob.Response.Id,
            300, 
            DateTime.UtcNow);

        var createTransactionUserToUserResult = 
            await CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserCommand);
        
        var openSupportTicketCommand = new OpenSupportTicketCommand(
            alice.Response.Id,
            alice.Response.WalletId,
            createTransactionUserToUserResult.Response.Id,
            "Please, can you rollback my transaction?",
            DateTime.UtcNow);
        
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
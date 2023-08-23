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

        var isSupportTicketExists = await DatabaseContextFixture.SupportTicketEntities
            .AnyAsync(x => x.Id == openSupportTicketResult.Response.Id);

        isSupportTicketExists.Should().BeTrue();
    }
}
using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetTicketsQueryHandlerTests;

public class GetTicketsTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var bob = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());

        var addCreditCardForAliceCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(alice.Response.Id);

        var addCreditCardForAliceResult = await AddCreditCardCommandHandler.HandleAsync(addCreditCardForAliceCommand);

        var createTransactionCardToUserForDimaCommand = new CreateTransactionCardToUserCommand(
            addCreditCardForAliceResult.Response.Id,
            alice.Response.Id,
            600,
            DateTime.UtcNow);

        await CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserForDimaCommand);
        
        var firstCreateTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            alice.Response.Id,
            bob.Response.Id,
            300, 
            DateTime.UtcNow);

        var secondCreateTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            alice.Response.Id,
            bob.Response.Id,
            300, 
            DateTime.UtcNow);
        
        var thirdCreateTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            bob.Response.Id,
            alice.Response.Id,
            300, 
            DateTime.UtcNow);
        
        var firstCreateTransactionUserToUserResult = 
            await CreateTransactionUserToUserCommandHandler.HandleAsync(firstCreateTransactionUserToUserCommand);
        var secondCreateTransactionUserToUserResult = 
            await CreateTransactionUserToUserCommandHandler.HandleAsync(secondCreateTransactionUserToUserCommand);
        var thirdCreateTransactionUserToUserResult = 
            await CreateTransactionUserToUserCommandHandler.HandleAsync(thirdCreateTransactionUserToUserCommand);
        
        var firstOpenSupportTicketForFirstTransactionCommand = new OpenSupportTicketCommand(
            alice.Response.Id,
            alice.Response.WalletId,
            firstCreateTransactionUserToUserResult.Response.Id,
            "Please, can you rollback my transaction?",
            DateTime.UtcNow);

        var secondOpenSupportTicketForFirstTransactionCommand = new OpenSupportTicketCommand(
            alice.Response.Id,
            alice.Response.WalletId,
            secondCreateTransactionUserToUserResult.Response.Id,
            "Please, can you rollback my transaction?",
            DateTime.UtcNow);
        
        var thirdOpenSupportTicketForFirstTransactionCommand = new OpenSupportTicketCommand(
            bob.Response.Id,
            bob.Response.WalletId,
            thirdCreateTransactionUserToUserResult.Response.Id,
            "Please, can you rollback my transaction?",
            DateTime.UtcNow);
        
        var firstOpenSupportTicketForFirstTransactionResult = 
            await OpenSupportTicketCommandHandler.HandleAsync(firstOpenSupportTicketForFirstTransactionCommand);
        var secondOpenSupportTicketForFirstTransactionResult = 
            await OpenSupportTicketCommandHandler.HandleAsync(secondOpenSupportTicketForFirstTransactionCommand);
        var thirdOpenSupportTicketForFirstTransactionResult =
        await OpenSupportTicketCommandHandler.HandleAsync(thirdOpenSupportTicketForFirstTransactionCommand);

        var getTicketsQuery = new GetTicketsQuery(dima.Response.Id, 10, DateTime.UtcNow);

        var getTicketsResult = await GetTicketsQueryHandler.HandleAsync(getTicketsQuery);

        getTicketsResult.Response.Count.Should().Be(3);
        getTicketsResult.Response
            .FirstOrDefault(x => x.Id == firstOpenSupportTicketForFirstTransactionResult.Response.Id)
            .Should().NotBeNull();
        getTicketsResult.Response
            .FirstOrDefault(x => x.Id == secondOpenSupportTicketForFirstTransactionResult.Response.Id)
            .Should().NotBeNull();
        getTicketsResult.Response
            .FirstOrDefault(x => x.Id == thirdOpenSupportTicketForFirstTransactionResult.Response.Id)
            .Should().NotBeNull();
    }
}
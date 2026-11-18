using FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;
using FluxPayAPI.Consumer.IntegrationTests.Helpers;
using FluxPayAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.CommandHandlerTests.OpenSupportTicketCommandHandlerTests;

public class OpenSupportTicketTestThrowConflictError(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestAddDoubleSupportTicket()
    {
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var bob = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());
        var addCreditCardForAliceCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(alice.Response.Id);
        var addCreditCardForAliceResult = await Fixture.AddCreditCardCommandHandler.HandleAsync(addCreditCardForAliceCommand);
        var createTransactionCardToUserForDimaCommand = new CreateTransactionCardToUserCommand(
            addCreditCardForAliceResult.Response.Id,
            alice.Response.Id,
            Amount: 300,
            DateTime.UtcNow);
        await Fixture.CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserForDimaCommand);
        var createTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            alice.Response.Id,
            bob.Response.Id,
            Amount: 300,
            DateTime.UtcNow);
        var createTransactionUserToUserResult =
            await Fixture.CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserCommand);
        var firstOpenSupportTicketCommand = new OpenSupportTicketCommand(
            alice.Response.Id,
            alice.Response.WalletId,
            createTransactionUserToUserResult.Response.Id,
            "Please, can you rollback my transaction?",
            DateTime.UtcNow);
        await Fixture.OpenSupportTicketCommandHandler.HandleAsync(firstOpenSupportTicketCommand);

        var secondOpenSupportTicketCommand = new OpenSupportTicketCommand(
            alice.Response.Id,
            alice.Response.WalletId,
            createTransactionUserToUserResult.Response.Id,
            "Please, can you rollback my transaction?",
            DateTime.UtcNow);
        var secondOpenSupportTicketResult = await Fixture.OpenSupportTicketCommandHandler.HandleAsync(secondOpenSupportTicketCommand);

        secondOpenSupportTicketResult.Error.Should().BeOfType<ConflictError>();
    }
}

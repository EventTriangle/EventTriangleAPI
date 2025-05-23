using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetSupportTicketsQueryHandlerTests;

public class GetSupportTicketsTestSuccess(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestSuccess()
    {
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var bob = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());
        var addCreditCardForAliceCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(alice.Response.Id);
        var addCreditCardForAliceResult = await Fixture.AddCreditCardCommandHandler.HandleAsync(addCreditCardForAliceCommand);
        var createTransactionCardToUserForDimaCommand = new CreateTransactionCardToUserCommand(
            addCreditCardForAliceResult.Response.Id,
            alice.Response.Id,
            600,
            DateTime.UtcNow);
        await Fixture.CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserForDimaCommand);
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
        var firstCreateTransactionUserToUserResult =
            await Fixture.CreateTransactionUserToUserCommandHandler.HandleAsync(firstCreateTransactionUserToUserCommand);
        var secondCreateTransactionUserToUserResult =
            await Fixture.CreateTransactionUserToUserCommandHandler.HandleAsync(secondCreateTransactionUserToUserCommand);
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
        var firstOpenSupportTicketForFirstTransactionResult =
            await Fixture.OpenSupportTicketCommandHandler.HandleAsync(firstOpenSupportTicketForFirstTransactionCommand);
        var secondOpenSupportTicketForFirstTransactionResult =
            await Fixture.OpenSupportTicketCommandHandler.HandleAsync(secondOpenSupportTicketForFirstTransactionCommand);

        var getSupportTicketsForAliceQuery = new GetSupportsTicketsQuery(alice.Response.Id, 10, DateTime.UtcNow);
        var getSupportTicketsForBobQuery = new GetSupportsTicketsQuery(bob.Response.Id, 10, DateTime.UtcNow);
        var getSupportTicketsForAliceResult = await Fixture.GetSupportTicketsQueryHandler.HandleAsync(getSupportTicketsForAliceQuery);
        var getSupportTicketsForBobResult = await Fixture.GetSupportTicketsQueryHandler.HandleAsync(getSupportTicketsForBobQuery);

        getSupportTicketsForAliceResult.Response.Count.Should().Be(2);
        getSupportTicketsForAliceResult.Response
            .FirstOrDefault(x => x.Id == firstOpenSupportTicketForFirstTransactionResult.Response.Id)
            .Should().NotBeNull();
        getSupportTicketsForAliceResult.Response
            .FirstOrDefault(x => x.Id == secondOpenSupportTicketForFirstTransactionResult.Response.Id)
            .Should().NotBeNull();
        getSupportTicketsForBobResult.Response.Count.Should().Be(0);
    }
}

using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetTicketsQueryHandlerTests;

public class GetTicketsTestSuccess(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
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
        var thirdCreateTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            bob.Response.Id,
            alice.Response.Id,
            300,
            DateTime.UtcNow);
        var firstCreateTransactionUserToUserResult =
            await Fixture.CreateTransactionUserToUserCommandHandler.HandleAsync(firstCreateTransactionUserToUserCommand);
        var secondCreateTransactionUserToUserResult =
            await Fixture.CreateTransactionUserToUserCommandHandler.HandleAsync(secondCreateTransactionUserToUserCommand);
        var thirdCreateTransactionUserToUserResult =
            await Fixture.CreateTransactionUserToUserCommandHandler.HandleAsync(thirdCreateTransactionUserToUserCommand);
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
            await Fixture.OpenSupportTicketCommandHandler.HandleAsync(firstOpenSupportTicketForFirstTransactionCommand);
        var secondOpenSupportTicketForFirstTransactionResult =
            await Fixture.OpenSupportTicketCommandHandler.HandleAsync(secondOpenSupportTicketForFirstTransactionCommand);
        var thirdOpenSupportTicketForFirstTransactionResult =
        await Fixture.OpenSupportTicketCommandHandler.HandleAsync(thirdOpenSupportTicketForFirstTransactionCommand);

        var getTicketsQuery = new GetTicketsQuery(dima.Response.Id, 10, DateTime.UtcNow);
        var getTicketsResult = await Fixture.GetTicketsQueryHandler.HandleAsync(getTicketsQuery);

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

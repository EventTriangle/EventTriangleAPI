using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.ResolveSupportTicketCommandHandlerTests;

public class ResolveSupportTicketTestThrowConflict(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterIsNotAdmin()
    {
        var bob = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
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
        var openSupportTicketCommand = new OpenSupportTicketCommand(
            alice.Response.Id,
            alice.Response.WalletId,
            createTransactionUserToUserResult.Response.Id,
            "Please, can you rollback my transaction?",
            DateTime.UtcNow);
        var openSupportTicketResult = await Fixture.OpenSupportTicketCommandHandler.HandleAsync(openSupportTicketCommand);

        var resolveSupportTicketCommand =  new ResolveSupportTicketCommand(
            bob.Response.Id,
            openSupportTicketResult.Response.Id,
            "Transaction is rolled back");
        var resolveSupportTicketResult = await Fixture.ResolveSupportTicketCommandHandler.HandleAsync(resolveSupportTicketCommand);

        resolveSupportTicketResult.Error.Should().BeOfType<ConflictError>();
    }
}

using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.ResolveSupportTicketCommandHandlerTests;

public class ResolveSupportTicketTestSuccess(TestFixture fixture) : TestBase(fixture)
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
        var supportTicket = await Fixture.DatabaseContextFixture.SupportTicketEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == openSupportTicketResult.Response.Id);

        var resolveSupportTicketCommand =  new ResolveSupportTicketCommand(
            dima.Response.Id,
            openSupportTicketResult.Response.Id,
            "Transaction is rolled back");
        await Fixture.ResolveSupportTicketCommandHandler.HandleAsync(resolveSupportTicketCommand);

        var supportTicketAfterResolving = await Fixture.DatabaseContextFixture.SupportTicketEntities
            .FirstOrDefaultAsync(x => x.Id == openSupportTicketResult.Response.Id);
        supportTicket.TicketStatus.Should().Be(TicketStatus.Open);
        supportTicketAfterResolving.TicketStatus.Should().Be(TicketStatus.Resolved);
    }
}

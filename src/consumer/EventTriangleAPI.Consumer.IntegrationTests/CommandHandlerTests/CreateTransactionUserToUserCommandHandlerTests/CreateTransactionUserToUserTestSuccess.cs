using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.CreateTransactionUserToUserCommandHandlerTests;

public class CreateTransactionUserToUserTestSuccess(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var addCreditCardForDimaCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(dima.Response.Id);
        var addCreditCardForDimaResult = await Fixture.AddCreditCardCommandHandler.HandleAsync(addCreditCardForDimaCommand);
        var createTransactionCardToUserForDimaCommand = new CreateTransactionCardToUserCommand(
            addCreditCardForDimaResult.Response.Id,
            dima.Response.Id,
            Amount: 300,
            DateTime.UtcNow);
        await Fixture.CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserForDimaCommand);

        var createTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            dima.Response.Id,
            alice.Response.Id,
            Amount: 300,
            DateTime.UtcNow);
        var createTransactionUserToUserResult =
            await Fixture.CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserCommand);

        var transaction = await Fixture.DatabaseContextFixture.TransactionEntities
            .FirstOrDefaultAsync(x => x.Id == createTransactionUserToUserResult.Response.Id);
        var dimaWallet = await Fixture.DatabaseContextFixture.WalletEntities.FirstOrDefaultAsync(x => x.Id == dima.Response.WalletId);
        var aliceWallet = await Fixture.DatabaseContextFixture.WalletEntities.FirstOrDefaultAsync(x => x.Id == alice.Response.WalletId);
        transaction.FromUserId.Should().Be(createTransactionUserToUserCommand.RequesterId);
        transaction.ToUserId.Should().Be(createTransactionUserToUserCommand.ToUserId);
        transaction.Amount.Should().Be(createTransactionUserToUserCommand.Amount);
        transaction.TransactionState.Should().Be(TransactionState.Completed);
        transaction.TransactionType.Should().Be(TransactionType.FromUserToUser);
        dimaWallet.Balance.Should().Be(0);
        aliceWallet.Balance.Should().Be(300);
    }
}

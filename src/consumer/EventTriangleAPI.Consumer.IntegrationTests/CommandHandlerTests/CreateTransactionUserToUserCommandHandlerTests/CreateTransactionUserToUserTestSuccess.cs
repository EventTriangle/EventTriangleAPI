using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.CreateTransactionUserToUserCommandHandlerTests;

public class CreateTransactionUserToUserTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var addCreditCardForDimaCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(dima.Response.Id);
        var addCreditCardForDimaResult = await AddCreditCardCommandHandler.HandleAsync(addCreditCardForDimaCommand);
        var createTransactionCardToUserForDimaCommand = new CreateTransactionCardToUserCommand(
            addCreditCardForDimaResult.Response.Id,
            dima.Response.Id,
            300,
            DateTime.UtcNow);
        await CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserForDimaCommand);
        
        var createTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            dima.Response.Id,
            alice.Response.Id,
            300, 
            DateTime.UtcNow);
        var createTransactionUserToUserResult = 
            await CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserCommand);

        var transaction = await DatabaseContextFixture.TransactionEntities
            .FirstOrDefaultAsync(x => x.Id == createTransactionUserToUserResult.Response.Id);
        var dimaWallet = await DatabaseContextFixture.WalletEntities.FirstOrDefaultAsync(x => x.Id == dima.Response.WalletId);
        var aliceWallet = await DatabaseContextFixture.WalletEntities.FirstOrDefaultAsync(x => x.Id == alice.Response.WalletId);
        transaction.FromUserId.Should().Be(createTransactionUserToUserCommand.RequesterId);
        transaction.ToUserId.Should().Be(createTransactionUserToUserCommand.ToUserId);
        transaction.Amount.Should().Be(createTransactionUserToUserCommand.Amount);
        transaction.TransactionState.Should().Be(TransactionState.Completed);
        transaction.TransactionType.Should().Be(TransactionType.FromUserToUser);
        dimaWallet.Balance.Should().Be(0);
        aliceWallet.Balance.Should().Be(300);
    }
}
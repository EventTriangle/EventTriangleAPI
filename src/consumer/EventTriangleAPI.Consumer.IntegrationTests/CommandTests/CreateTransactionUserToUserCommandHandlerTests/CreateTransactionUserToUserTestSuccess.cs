using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandTests.CreateTransactionUserToUserCommandHandlerTests;

public class CreateTransactionUserToUserTestSuccess : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserAliceCommand());
        
        var addCreditCardForDimaCommand = new AddCreditCardCommand(
            dima.Response.Id,
            "Dima",
            "1234567890123456",
            "123",
            "04/12",
            PaymentNetwork.MasterCard);

        var addCreditCardForDimaResult = await AddCreditCardCommandHandler.HandleAsync(addCreditCardForDimaCommand);

        var createTransactionCardToUserForDimaCommand = new CreateTransactionCardToUserCommand(
            addCreditCardForDimaResult.Response.Id,
            dima.Response.Id,
            300);

        await CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserForDimaCommand);
        
        var createTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            dima.Response.Id,
            alice.Response.Id,
            300);

        var createTransactionUserToUserResult = 
            await CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserCommand);

        var transaction = await DatabaseContextFixture.TransactionEntities
            .FirstOrDefaultAsync(x => x.Id == createTransactionUserToUserResult.Response.Id);

        var dimaWallet = await DatabaseContextFixture.WalletEntities.FirstOrDefaultAsync(x => x.Id == dima.Response.WalletId);
        var aliceWallet = await DatabaseContextFixture.WalletEntities.FirstOrDefaultAsync(x => x.Id == alice.Response.WalletId);
        
        transaction.FromUserId.Should().Be(createTransactionUserToUserCommand.FromUserId);
        transaction.ToUserId.Should().Be(createTransactionUserToUserCommand.ToUserId);
        transaction.Amount.Should().Be(createTransactionUserToUserCommand.Amount);
        transaction.TransactionState.Should().Be(TransactionState.Completed);
        transaction.TransactionType.Should().Be(TransactionType.FromUserToUser);

        dimaWallet.Balance.Should().Be(0);
        aliceWallet.Balance.Should().Be(300);
    }
}
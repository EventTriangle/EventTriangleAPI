using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.RollBackTransactionCommandHandlerTests;

public class RollBackTransactionTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserAliceCommand());
        var bob = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserBobCommand());

        var addCreditCardCommand = new AddCreditCardCommand(
            bob.Response.Id,
            "Bob",
            "1111222233334444",
            "123",
            "12/11",
            PaymentNetwork.Visa);

        var addCreditCardResult = await AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);
        
        var createTransactionCardToUserCommand = new CreateTransactionCardToUserCommand(
            addCreditCardResult.Response.Id,
            bob.Response.Id,
            300);

        await CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserCommand);
        
        var createTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            bob.Response.Id,
            alice.Response.Id,
            300);

        var  createTransactionUserToUserResult = await CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserCommand);

        var rollbackTransactionCommand = new RollBackTransactionCommand(
            dima.Response.Id, 
            createTransactionUserToUserResult.Response.Id);

        await RollBackTransactionCommandHandler.HandleAsync(rollbackTransactionCommand);

        var transaction = await DatabaseContextFixture.TransactionEntities
            .FirstAsync(x => x.Id == createTransactionUserToUserResult.Response.Id);

        transaction.TransactionState.Should().Be(TransactionState.RolledBack);
    }
}
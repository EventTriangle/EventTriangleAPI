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
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var bob = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());
        var addCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(dima.Response.Id);
        var addCreditCardResult = await AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);
        var createTransactionCardToUserCommand = new CreateTransactionCardToUserCommand(
            addCreditCardResult.Response.Id,
            bob.Response.Id,
            Amount: 300,
            DateTime.UtcNow);
        await CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserCommand);
        var createTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            bob.Response.Id,
            alice.Response.Id,
            Amount: 300,
            DateTime.UtcNow);
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
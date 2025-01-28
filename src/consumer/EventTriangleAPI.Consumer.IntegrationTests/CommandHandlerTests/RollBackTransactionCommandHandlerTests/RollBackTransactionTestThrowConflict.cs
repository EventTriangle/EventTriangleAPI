using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.RollBackTransactionCommandHandlerTests;

public class RollBackTransactionTestThrowConflict(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterIsNotAdmin()
    {
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var bob = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());
        var addCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(bob.Response.Id);
        var addCreditCardResult = await Fixture.AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);
        var createTransactionCardToUserCommand = new CreateTransactionCardToUserCommand(
            addCreditCardResult.Response.Id,
            bob.Response.Id,
            Amount: 300,
            DateTime.UtcNow);
        await Fixture.CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserCommand);
        var createTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            bob.Response.Id,
            alice.Response.Id,
            Amount: 300,
            DateTime.UtcNow);
        var  createTransactionUserToUserResult = await Fixture.CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserCommand);

        var rollbackTransactionCommand = new RollBackTransactionCommand(
            bob.Response.Id,
            createTransactionUserToUserResult.Response.Id);
        var rollbackTransactionResult = await Fixture.RollBackTransactionCommandHandler.HandleAsync(rollbackTransactionCommand);

        rollbackTransactionResult.Error.Should().BeOfType<ConflictError>();
    }

    [Fact]
    public async Task TestTransactionHasAlreadyBeenRolledBack()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var bob = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());
        var addCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(bob.Response.Id);
        var addCreditCardResult = await Fixture.AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);
        var createTransactionCardToUserCommand = new CreateTransactionCardToUserCommand(
            addCreditCardResult.Response.Id,
            bob.Response.Id,
            Amount: 300,
            DateTime.UtcNow);
        await Fixture.CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserCommand);
        var createTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            bob.Response.Id,
            alice.Response.Id,
            Amount: 300,
            DateTime.UtcNow);
        var createTransactionUserToUserResult = await Fixture.CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserCommand);
        var firstRollbackTransactionCommand = new RollBackTransactionCommand(
            dima.Response.Id,
            createTransactionUserToUserResult.Response.Id);
        await Fixture.RollBackTransactionCommandHandler.HandleAsync(firstRollbackTransactionCommand);

        var secondRollbackTransactionCommand = new RollBackTransactionCommand(
            dima.Response.Id,
            createTransactionUserToUserResult.Response.Id);
        var secondRollbackTransactionResult = await Fixture.RollBackTransactionCommandHandler.HandleAsync(secondRollbackTransactionCommand);

        secondRollbackTransactionResult.Error.Should().BeOfType<ConflictError>();
    }
}

using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.CreateTransactionUserToUserCommandHandlerTests;

public class CreateTransactionUserToUserTestThrowConflict(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestTransactionAmountGreaterThanBalance()
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
            Amount: 1000,
            DateTime.UtcNow);
        var createTransactionUserToUserResult =
            await Fixture.CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserCommand);

        createTransactionUserToUserResult.Error.Should().BeOfType<ConflictError>();
    }

    [Fact]
    public async Task RequesterSuspended()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var bob = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var addCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(alice.Response.Id);
        var addCreditCardResult = await Fixture.AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);
        var createTransactionCardToUserCommand = new CreateTransactionCardToUserCommand(
            addCreditCardResult.Response.Id,
            alice.Response.Id,
            Amount: 300,
            DateTime.UtcNow);
        await Fixture.CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserCommand);
        var suspendUserCommand = new SuspendUserCommand(dima.Response.Id, alice.Response.Id);
        await Fixture.SuspendUserCommandHandler.HandleAsync(suspendUserCommand);

        var createTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            alice.Response.Id,
            bob.Response.Id,
            Amount: 300,
            DateTime.Now);
        var createTransactionUserToUserResult =
            await Fixture.CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserCommand);

        createTransactionUserToUserResult.Error.Should().BeOfType<ConflictError>();
    }
}

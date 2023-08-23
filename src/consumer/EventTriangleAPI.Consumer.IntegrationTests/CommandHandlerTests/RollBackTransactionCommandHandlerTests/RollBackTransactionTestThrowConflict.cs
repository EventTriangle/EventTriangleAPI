using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.RollBackTransactionCommandHandlerTests;

public class RollBackTransactionTestThrowConflict : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterIsNotAdmin()
    {
        var alice = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var bob = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());

        var addCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(bob.Response.Id);

        var addCreditCardResult = await AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);
        
        var createTransactionCardToUserCommand = new CreateTransactionCardToUserCommand(
            addCreditCardResult.Response.Id,
            bob.Response.Id,
            300,
            DateTime.UtcNow);

        await CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserCommand);
        
        var createTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            bob.Response.Id,
            alice.Response.Id,
            300,
            DateTime.UtcNow);

        var  createTransactionUserToUserResult = await CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserCommand);

        var rollbackTransactionCommand = new RollBackTransactionCommand(
            bob.Response.Id, 
            createTransactionUserToUserResult.Response.Id);

        var rollbackTransactionResult = await RollBackTransactionCommandHandler.HandleAsync(rollbackTransactionCommand);

        rollbackTransactionResult.Error.Should().BeOfType<ConflictError>();
    }
}
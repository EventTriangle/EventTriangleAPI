using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.RollBackTransactionCommandHandlerTests;

public class RollBackTransactionTestThrowConflict : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterIsNotAdmin()
    {
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
            bob.Response.Id, 
            createTransactionUserToUserResult.Response.Id);

        var rollbackTransactionResult = await RollBackTransactionCommandHandler.HandleAsync(rollbackTransactionCommand);

        rollbackTransactionResult.Error.Should().BeOfType<ConflictError>();
    }
}
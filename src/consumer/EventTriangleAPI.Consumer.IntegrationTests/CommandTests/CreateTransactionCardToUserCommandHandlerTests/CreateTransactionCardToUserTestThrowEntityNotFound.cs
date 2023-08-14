using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandTests.CreateTransactionCardToUserCommandHandlerTests;

public class CreateTransactionCardToUserTestThrowEntityNotFound : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserDimaCommand());
        
        var addCreditCardCommand = new AddCreditCardCommand(
            dima.Response.Id,
            "Dima",
            "1234567890123456",
            "123",
            "04/12",
            PaymentNetwork.MasterCard);

        var addCreditCardResult = await AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);

        var createTransactionCardToUserByNobodyCommand = new CreateTransactionCardToUserCommand(
            addCreditCardResult.Response.Id,
            Guid.NewGuid().ToString(),
            300);
        
        var createTransactionCardToUserWithNonExistentCardCommand = new CreateTransactionCardToUserCommand(
            Guid.NewGuid(),
            dima.Response.Id,
            300);

        var createTransactionCardToUserByNobodyResult = 
            await CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserByNobodyCommand);
        
        var createTransactionCardToUserWithNonExistentCardResult = 
            await CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserWithNonExistentCardCommand);

        createTransactionCardToUserByNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
        createTransactionCardToUserWithNonExistentCardResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
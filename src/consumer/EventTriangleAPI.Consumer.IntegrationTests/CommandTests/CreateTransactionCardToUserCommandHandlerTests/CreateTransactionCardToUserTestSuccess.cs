using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandTests.CreateTransactionCardToUserCommandHandlerTests;

public class CreateTransactionCardToUserTestSuccess : IntegrationTestBase, IIntegrationTest
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

        var createTransactionCardToUserCommand = new CreateTransactionCardToUserCommand(
            addCreditCardResult.Response.Id,
            dima.Response.Id,
            300);

        var createTransactionCardToUserResult = 
            await CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserCommand);

        var transaction = await DatabaseContextFixture.TransactionEntities
            .FirstOrDefaultAsync(x => x.Id == createTransactionCardToUserResult.Response.Id);

        transaction.FromUserId.Should().Be(createTransactionCardToUserCommand.ToUserId);
        transaction.ToUserId.Should().Be(createTransactionCardToUserCommand.ToUserId);
        transaction.Amount.Should().Be(createTransactionCardToUserCommand.Amount);
        transaction.TransactionState.Should().Be(TransactionState.Completed);
        transaction.TransactionType.Should().Be(TransactionType.FromCardToUser);
    }
}
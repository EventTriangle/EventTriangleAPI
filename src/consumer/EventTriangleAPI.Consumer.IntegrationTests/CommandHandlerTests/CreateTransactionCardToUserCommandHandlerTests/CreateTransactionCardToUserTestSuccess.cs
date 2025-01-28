using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.CreateTransactionCardToUserCommandHandlerTests;

public class CreateTransactionCardToUserTestSuccess(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var addCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(dima.Response.Id);
        var addCreditCardResult = await Fixture.AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);

        var createTransactionCardToUserCommand = new CreateTransactionCardToUserCommand(
            addCreditCardResult.Response.Id,
            dima.Response.Id,
            Amount: 300,
            DateTime.UtcNow);
        var createTransactionCardToUserResult =
            await Fixture.CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserCommand);

        var transaction = await Fixture.DatabaseContextFixture.TransactionEntities
            .FirstOrDefaultAsync(x => x.Id == createTransactionCardToUserResult.Response.Id);
        transaction.FromUserId.Should().Be(createTransactionCardToUserCommand.RequesterId);
        transaction.ToUserId.Should().Be(createTransactionCardToUserCommand.RequesterId);
        transaction.Amount.Should().Be(createTransactionCardToUserCommand.Amount);
        transaction.TransactionState.Should().Be(TransactionState.Completed);
        transaction.TransactionType.Should().Be(TransactionType.FromCardToUser);
    }
}

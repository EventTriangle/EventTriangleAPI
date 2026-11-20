using FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;
using FluxPayAPI.Consumer.IntegrationTests.Helpers;
using FluxPayAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.CommandHandlerTests.CreateTransactionCardToUserCommandHandlerTests;

public class CreateTransactionCardToUserTestThrowEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var addCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(dima.Response.Id);
        var addCreditCardResult = await Fixture.AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);

        var createTransactionCardToUserByNobodyCommand = new CreateTransactionCardToUserCommand(
            addCreditCardResult.Response.Id,
            Guid.NewGuid().ToString(),
            Amount: 300,
            DateTime.UtcNow);
        var createTransactionCardToUserByNobodyResult =
            await Fixture.CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserByNobodyCommand);

        createTransactionCardToUserByNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }

    [Fact]
    public async Task TestCreditCardNotFound()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var createTransactionCardToUserWithNonExistentCardCommand = new CreateTransactionCardToUserCommand(
        Guid.NewGuid(),
        dima.Response.Id,
        Amount: 300,
        DateTime.UtcNow);

        var createTransactionCardToUserWithNonExistentCardResult =
        await Fixture.CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserWithNonExistentCardCommand);

        createTransactionCardToUserWithNonExistentCardResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }

    [Fact]
    public async Task TestCannotUseAnotherUsersCard()
    {
        var owner = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var requester = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());
        var card = await Fixture.AddCreditCardCommandHandler.HandleAsync(AddCreditCardCommandHelper.CreateCreditCardCommand(owner.Response.Id));

        var result = await Fixture.CreateTransactionCardToUserCommandHandler.HandleAsync(
            new CreateTransactionCardToUserCommand(card.Response.Id, requester.Response.Id, 1, DateTime.UtcNow));

        result.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

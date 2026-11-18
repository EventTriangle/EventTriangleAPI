using FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;
using FluxPayAPI.Consumer.IntegrationTests.Helpers;
using FluxPayAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.CommandHandlerTests.CreateTransactionCardToUserCommandHandlerTests;

public class CreateTransactionCardToUserTestThrowConflict(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task RequesterIsSuspended()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var addCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(alice.Response.Id);
        var addCreditCardResult = await Fixture.AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);
        var suspendUserCommand = new SuspendUserCommand(dima.Response.Id, alice.Response.Id);
        await Fixture.SuspendUserCommandHandler.HandleAsync(suspendUserCommand);

        var createTransactionCardToUserCommand = new CreateTransactionCardToUserCommand(
            addCreditCardResult.Response.Id,
            alice.Response.Id,
            Amount: 300,
            DateTime.UtcNow);
        var createTransactionCardToUserResult =
            await Fixture.CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserCommand);

        createTransactionCardToUserResult.Error.Should().BeOfType<ConflictError>();
    }
}

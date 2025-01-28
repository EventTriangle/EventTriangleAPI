using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetTransactionsByUserIdQueryHandlerTests;

public class GetTransactionsByUserIdTestSuccess(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var bob = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var addCreditCardForBobCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(bob.Response.Id);
        var addCreditCardForBobResult = await Fixture.AddCreditCardCommandHandler.HandleAsync(addCreditCardForBobCommand);
        var createTransactionCardToUserForBobCommand = new CreateTransactionCardToUserCommand(
            addCreditCardForBobResult.Response.Id,
            bob.Response.Id,
            300,
            DateTime.UtcNow);
        await Fixture.CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserForBobCommand);
        var createTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            bob.Response.Id,
            alice.Response.Id,
            300,
            DateTime.UtcNow);
        var createTransactionUserToUserResult =
            await Fixture.CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserCommand);

        var getTransactionsByUserIdBobQuery = new GetTransactionsByUserIdQuery(dima.Response.Id, bob.Response.Id, 10, DateTime.UtcNow);
        var getTransactionsByUserIdBobResult = await Fixture.GetTransactionsByUserIdQueryHandler.HandleAsync(getTransactionsByUserIdBobQuery);
        var getTransactionsByUserIdAliceQuery = new GetTransactionsByUserIdQuery(dima.Response.Id, alice.Response.Id, 10, DateTime.UtcNow);
        var getTransactionsByUserIdAliceResult = await Fixture.GetTransactionsByUserIdQueryHandler.HandleAsync(getTransactionsByUserIdAliceQuery);

        getTransactionsByUserIdBobResult.Response.Count.Should().Be(2);
        getTransactionsByUserIdBobResult.Response
            .FirstOrDefault(x => x.Id == createTransactionUserToUserResult.Response.Id)
            .Should().NotBeNull();
        getTransactionsByUserIdAliceResult.Response.Count.Should().Be(1);
        getTransactionsByUserIdAliceResult.Response
            .FirstOrDefault(x => x.Id == createTransactionUserToUserResult.Response.Id)
            .Should().NotBeNull();
    }
}

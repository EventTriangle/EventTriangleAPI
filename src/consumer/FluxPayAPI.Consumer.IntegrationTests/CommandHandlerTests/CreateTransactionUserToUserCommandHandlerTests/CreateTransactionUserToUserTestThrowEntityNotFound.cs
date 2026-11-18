using FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;
using FluxPayAPI.Consumer.IntegrationTests.Helpers;
using FluxPayAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.CommandHandlerTests.CreateTransactionUserToUserCommandHandlerTests;

public class CreateTransactionUserToUserTestThrowEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());

        var createTransactionUserToUserByNobodyCommand = new CreateTransactionUserToUserCommand(
            Guid.NewGuid().ToString(),
            alice.Response.Id,
            Amount: 300,
            DateTime.UtcNow);
        var createTransactionUserToUserByNobodyResult =
            await Fixture.CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserByNobodyCommand);

        createTransactionUserToUserByNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }

    [Fact]
    public async Task TestUserNotFound()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var createTransactionUserToUserForNobodyCommand = new CreateTransactionUserToUserCommand(
            dima.Response.Id,
            Guid.NewGuid().ToString(),
            Amount: 300,
            DateTime.UtcNow);
        var createTransactionUserToUserForNobodyResult =
            await Fixture.CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserForNobodyCommand);

        createTransactionUserToUserForNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

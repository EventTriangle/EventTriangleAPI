using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandTests.CreateTransactionUserToUserCommandHandlerTests;

public class CreateTransactionUserToUserTestThrowEntityNotFound : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserAliceCommand());

        var createTransactionUserToUserByNobodyCommand = new CreateTransactionUserToUserCommand(
            Guid.NewGuid().ToString(),
            alice.Response.Id,
            300);
        
        var createTransactionUserToUserForNobodyCommand = new CreateTransactionUserToUserCommand(
            dima.Response.Id,
            Guid.NewGuid().ToString(),
            300);

        var createTransactionUserToUserByNobodyResult = 
            await CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserByNobodyCommand);
        
        var createTransactionUserToUserForNobodyResult = 
            await CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserForNobodyCommand);

        createTransactionUserToUserByNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
        createTransactionUserToUserForNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
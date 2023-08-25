using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.CreateTransactionUserToUserCommandHandlerTests;

public class CreateTransactionUserToUserTestThrowEntityNotFound : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var alice = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        
        var createTransactionUserToUserByNobodyCommand = new CreateTransactionUserToUserCommand(
            Guid.NewGuid().ToString(),
            alice.Response.Id,
            300,
            DateTime.UtcNow);
        var createTransactionUserToUserByNobodyResult = 
            await CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserByNobodyCommand);
        
        createTransactionUserToUserByNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
    
    [Fact]
    public async Task TestUserNotFound()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var createTransactionUserToUserForNobodyCommand = new CreateTransactionUserToUserCommand(
            dima.Response.Id,
            Guid.NewGuid().ToString(),
            300,
            DateTime.UtcNow);
        var createTransactionUserToUserForNobodyResult = 
            await CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserForNobodyCommand);

        createTransactionUserToUserForNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
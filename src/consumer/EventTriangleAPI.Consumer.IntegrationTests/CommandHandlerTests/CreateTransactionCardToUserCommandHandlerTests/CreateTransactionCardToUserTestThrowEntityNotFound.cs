using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.CreateTransactionCardToUserCommandHandlerTests;

public class CreateTransactionCardToUserTestThrowEntityNotFound : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var addCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(dima.Response.Id);
        var addCreditCardResult = await AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);

        var createTransactionCardToUserByNobodyCommand = new CreateTransactionCardToUserCommand(
            addCreditCardResult.Response.Id,
            Guid.NewGuid().ToString(),
            300,
            DateTime.UtcNow);
        var createTransactionCardToUserByNobodyResult = 
            await CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserByNobodyCommand);

        createTransactionCardToUserByNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
    
    [Fact]
    public async Task TestCreditCardNotFound()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        
        var createTransactionCardToUserWithNonExistentCardCommand = new CreateTransactionCardToUserCommand(
        Guid.NewGuid(),
        dima.Response.Id,
        300,
        DateTime.UtcNow);

        var createTransactionCardToUserWithNonExistentCardResult = 
        await CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserWithNonExistentCardCommand);

        createTransactionCardToUserWithNonExistentCardResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
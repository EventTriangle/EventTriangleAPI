using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetTransactionsBySearchQueryHandlerTests;

public class GetTransactionsBySearchTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var addCreditCardForDimaCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(dima.Response.Id);
        var addCreditCardForDimaResult = await AddCreditCardCommandHandler.HandleAsync(addCreditCardForDimaCommand);
        var createTransactionCardToUserForDimaCommand = new CreateTransactionCardToUserCommand(
            addCreditCardForDimaResult.Response.Id,
            dima.Response.Id,
            300,
            DateTime.UtcNow);
        await CreateTransactionCardToUserCommandHandler.HandleAsync(createTransactionCardToUserForDimaCommand);
        var createTransactionUserToUserCommand = new CreateTransactionUserToUserCommand(
            dima.Response.Id,
            alice.Response.Id,
            300, 
            DateTime.UtcNow);
        var createTransactionUserToUserResult = 
            await CreateTransactionUserToUserCommandHandler.HandleAsync(createTransactionUserToUserCommand);
        var searchText = createTransactionUserToUserResult.Response.Id.ToString()[..6];
        
        var getTransactionsForDimaQuery = new GetTransactionsBySearchQuery(dima.Response.Id, searchText, 10, DateTime.UtcNow);
        var getTransactionsForAliceQuery = new GetTransactionsBySearchQuery(alice.Response.Id, searchText, 10, DateTime.UtcNow);
        var getTransactionsForDimaResult = await GetTransactionsBySearchQueryHandler.HandleAsync(getTransactionsForDimaQuery);
        var getTransactionsForAliceResult = await GetTransactionsBySearchQueryHandler.HandleAsync(getTransactionsForAliceQuery);

        getTransactionsForDimaResult.Response.Count.Should().Be(1);
        getTransactionsForDimaResult.Response
            .FirstOrDefault(x => x.Id == createTransactionUserToUserResult.Response.Id)
            .Should().NotBeNull();
        getTransactionsForAliceResult.Response.Count.Should().Be(1);
        getTransactionsForAliceResult.Response
            .FirstOrDefault(x => x.Id == createTransactionUserToUserResult.Response.Id)
            .Should().NotBeNull();
    }
}
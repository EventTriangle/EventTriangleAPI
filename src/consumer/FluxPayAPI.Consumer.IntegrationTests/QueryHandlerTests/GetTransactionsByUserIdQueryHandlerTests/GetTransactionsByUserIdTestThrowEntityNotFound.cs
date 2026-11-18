using FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;
using FluxPayAPI.Consumer.IntegrationTests.Helpers;
using FluxPayAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.QueryHandlerTests.GetTransactionsByUserIdQueryHandlerTests;

public class GetTransactionsByUserIdTestThrowEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestUserNotFound()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var getTransactionsQuery = new GetTransactionsByUserIdQuery(dima.Response.Id, Guid.NewGuid().ToString(), 10, DateTime.UtcNow);
        var getTransactionsResult = await Fixture.GetTransactionsByUserIdQueryHandler.HandleAsync(getTransactionsQuery);

        getTransactionsResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

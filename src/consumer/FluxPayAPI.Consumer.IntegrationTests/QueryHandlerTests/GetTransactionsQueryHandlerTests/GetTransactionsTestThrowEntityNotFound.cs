using FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;
using FluxPayAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.QueryHandlerTests.GetTransactionsQueryHandlerTests;

public class GetTransactionsTestThrowEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var getTransactionsQuery = new GetTransactionsQuery(Guid.NewGuid().ToString(), 10, DateTime.UtcNow);
        var getTransactionsResult = await Fixture.GetTransactionsQueryHandler.HandleAsync(getTransactionsQuery);

        getTransactionsResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

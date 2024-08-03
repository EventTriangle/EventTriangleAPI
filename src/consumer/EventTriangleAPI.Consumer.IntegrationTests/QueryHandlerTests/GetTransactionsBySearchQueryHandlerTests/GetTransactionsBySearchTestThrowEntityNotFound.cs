using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetTransactionsBySearchQueryHandlerTests;

public class GetTransactionsTestBySearchThrowEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var getTransactionsQuery = new GetTransactionsBySearchQuery(Guid.NewGuid().ToString(), string.Empty, 10, DateTime.UtcNow);
        var getTransactionsResult = await Fixture.GetTransactionsBySearchQueryHandler.HandleAsync(getTransactionsQuery);

        getTransactionsResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetTransactionsQueryHandlerTests;

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

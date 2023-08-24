using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetTicketsQueryHandlerTests;

public class GetTicketsTestEntityNotFound : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var getTicketsQuery = new GetTicketsQuery(Guid.NewGuid().ToString(), 10, DateTime.UtcNow);

        var getTicketsResult = await GetTicketsQueryHandler.HandleAsync(getTicketsQuery);

        getTicketsResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
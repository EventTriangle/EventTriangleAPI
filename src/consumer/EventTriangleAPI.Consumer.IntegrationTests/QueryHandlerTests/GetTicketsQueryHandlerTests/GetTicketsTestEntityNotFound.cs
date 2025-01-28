using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetTicketsQueryHandlerTests;

public class GetTicketsTestEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var getTicketsQuery = new GetTicketsQuery(Guid.NewGuid().ToString(), 10, DateTime.UtcNow);
        var getTicketsResult = await Fixture.GetTicketsQueryHandler.HandleAsync(getTicketsQuery);

        getTicketsResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

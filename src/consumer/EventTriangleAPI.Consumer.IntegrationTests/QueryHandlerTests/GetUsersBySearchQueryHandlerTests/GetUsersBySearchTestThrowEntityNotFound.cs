using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetUsersBySearchQueryHandlerTests;

public class GetUsersBySearchTestThrowEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var getUsersBySearchQuery = new GetUsersBySearchQuery(Guid.NewGuid().ToString(), "bo", 10, 1);
        var getUsersBySearchResult = await Fixture.GetUsersBySearchQueryHandler.HandleAsync(getUsersBySearchQuery);

        getUsersBySearchResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetContactsBySearchQueryHandlerTests;

public class GetContactsBySearchTestEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var getContactsBySearchQuery = new GetContactsBySearchQuery(Guid.NewGuid().ToString(), "user");
        var getContactsBySearchResult = await Fixture.GetContactsBySearchQueryHandler.HandleAsync(getContactsBySearchQuery);

        getContactsBySearchResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

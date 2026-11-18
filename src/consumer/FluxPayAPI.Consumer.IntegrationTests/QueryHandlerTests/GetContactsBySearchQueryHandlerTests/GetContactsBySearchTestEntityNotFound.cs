using FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;
using FluxPayAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.QueryHandlerTests.GetContactsBySearchQueryHandlerTests;

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

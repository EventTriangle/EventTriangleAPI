using FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;
using FluxPayAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.QueryHandlerTests.GetContactsQueryHandlerTests;

public class GetContactsTestEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var getContactsQuery = new GetContactsQuery(Guid.NewGuid().ToString());
        var getContactsResult = await Fixture.GetContactsQueryHandler.HandleAsync(getContactsQuery);

        getContactsResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

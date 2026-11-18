using FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;
using FluxPayAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.QueryHandlerTests.GetUsersQueryHandlerTests;

public class GetUsersTestThrowEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterIsNotFound()
    {
        var getUsersQuery = new GetUsersQuery(Guid.NewGuid().ToString(), 10, 1);
        var getUsersResult = await Fixture.GetUsersQueryHandler.HandleAsync(getUsersQuery);

        getUsersResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

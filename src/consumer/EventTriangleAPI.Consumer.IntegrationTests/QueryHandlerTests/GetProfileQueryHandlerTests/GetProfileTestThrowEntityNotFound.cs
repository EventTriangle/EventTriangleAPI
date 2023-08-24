using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetProfileQueryHandlerTests;

public class GetProfileTestThrowEntityNotFound : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var getProfileQuery = new GetProfileQuery(Guid.NewGuid().ToString());

        var getProfileResult = await GetProfileQueryHandler.HandleAsync(getProfileQuery);

        getProfileResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
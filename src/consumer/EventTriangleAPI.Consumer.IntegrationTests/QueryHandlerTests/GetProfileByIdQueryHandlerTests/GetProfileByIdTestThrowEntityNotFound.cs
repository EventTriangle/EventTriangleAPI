using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetProfileByIdQueryHandlerTests;

public class GetProfileByIdTestThrowEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestUserNotFound()
    {
        var getProfileByIdQuery = new GetProfileByIdQuery(Guid.NewGuid().ToString());
        var getProfileByIdResult = await Fixture.GetProfileByIdQueryHandler.HandleAsync(getProfileByIdQuery);

        getProfileByIdResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

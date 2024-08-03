using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetUsersBySearchQueryHandlerTests;

public class GetUsersBySearchTestBadRequest(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestPageLessThanOne()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var getUsersBySearchQuery = new GetUsersBySearchQuery(dima.Response.Id, "ali", 10, -10);
        var getUsersBySearchResult = await Fixture.GetUsersBySearchQueryHandler.HandleAsync(getUsersBySearchQuery);

        getUsersBySearchResult.Error.Should().BeOfType<BadRequestError>();
    }
}

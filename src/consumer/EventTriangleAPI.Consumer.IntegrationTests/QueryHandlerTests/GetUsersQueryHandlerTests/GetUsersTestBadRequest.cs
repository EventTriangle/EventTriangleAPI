using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetUsersQueryHandlerTests;

public class GetUsersTestBadRequest(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestPageLessThanOne()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var getUsersQuery = new GetUsersQuery(dima.Response.Id, 10, -10);
        var getUsersResult = await Fixture.GetUsersQueryHandler.HandleAsync(getUsersQuery);

        getUsersResult.Error.Should().BeOfType<BadRequestError>();
    }
}

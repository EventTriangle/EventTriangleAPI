using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetUsersQueryHandlerTests;

public class GetUsersTestSuccess(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var bob = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());

        var getUsersQuery = new GetUsersQuery(dima.Response.Id, 10, 1);
        var getUsersResult = await Fixture.GetUsersQueryHandler.HandleAsync(getUsersQuery);

        getUsersResult.Response.Count.Should().Be(2);
        getUsersResult.Response.FirstOrDefault(x => x.Id == alice.Response.Id).Should().NotBeNull();
        getUsersResult.Response.FirstOrDefault(x => x.Id == bob.Response.Id).Should().NotBeNull();
    }
}

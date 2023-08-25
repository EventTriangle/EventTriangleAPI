using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetUsersQueryHandlerTests;

public class GetUsersTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var bob = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());

        var getUsersQuery = new GetUsersQuery(dima.Response.Id, 10, 1);
        var getUsersResult = await GetUsersQueryHandler.HandleAsync(getUsersQuery);

        getUsersResult.Response.Count.Should().Be(2);
        getUsersResult.Response.FirstOrDefault(x => x.Id == alice.Response.Id).Should().NotBeNull();
        getUsersResult.Response.FirstOrDefault(x => x.Id == bob.Response.Id).Should().NotBeNull();
    }
}
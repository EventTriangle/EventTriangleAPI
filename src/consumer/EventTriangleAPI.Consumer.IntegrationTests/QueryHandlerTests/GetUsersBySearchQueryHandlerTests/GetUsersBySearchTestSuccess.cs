using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetUsersBySearchQueryHandlerTests;

public class GetUsersBySearchTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var bob = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());

        var getUsersBySearchAliceQuery = new GetUsersBySearchQuery(dima.Response.Id, "ali", 10, 1);
        var getUsersBySearchBobQuery = new GetUsersBySearchQuery(dima.Response.Id, "bo", 10, 1);

        var getUsersBySearchAliceResult = await GetUsersBySearchQueryHandler.HandleAsync(getUsersBySearchAliceQuery);
        var getUsersBySearchBobResult = await GetUsersBySearchQueryHandler.HandleAsync(getUsersBySearchBobQuery);

        getUsersBySearchAliceResult.Response.Count.Should().Be(1);
        getUsersBySearchBobResult.Response.Count.Should().Be(1);
        
        getUsersBySearchAliceResult.Response.FirstOrDefault(x => x.Id == alice.Response.Id).Should().NotBeNull();
        getUsersBySearchBobResult.Response.FirstOrDefault(x => x.Id == bob.Response.Id).Should().NotBeNull();
    }
}
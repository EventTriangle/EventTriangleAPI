using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetUsersQueryHandlerTests;

public class GetUsersTestThrowConflict(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterIsNotAdmin()
    {
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());

        var getUsersQuery = new GetUsersQuery(alice.Response.Id, 10, 1);
        var getUsersResult = await Fixture.GetUsersQueryHandler.HandleAsync(getUsersQuery);

        getUsersResult.Error.Should().BeOfType<ConflictError>();
    }
}

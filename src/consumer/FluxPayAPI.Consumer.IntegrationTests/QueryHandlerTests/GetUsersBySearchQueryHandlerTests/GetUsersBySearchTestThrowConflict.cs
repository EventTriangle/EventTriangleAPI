using FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;
using FluxPayAPI.Consumer.IntegrationTests.Helpers;
using FluxPayAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.QueryHandlerTests.GetUsersBySearchQueryHandlerTests;

public class GetUsersBySearchTestThrowConflict(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterIsNotAdmin()
    {
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());

        var getUsersBySearchQuery = new GetUsersBySearchQuery(alice.Response.Id, "bo", 10, 1);
        var getUsersBySearchResult = await Fixture.GetUsersBySearchQueryHandler.HandleAsync(getUsersBySearchQuery);

        getUsersBySearchResult.Error.Should().BeOfType<ConflictError>();
    }
}

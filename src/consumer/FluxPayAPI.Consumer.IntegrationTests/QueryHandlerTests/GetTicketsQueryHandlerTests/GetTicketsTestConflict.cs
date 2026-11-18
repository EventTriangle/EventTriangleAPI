using FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;
using FluxPayAPI.Consumer.IntegrationTests.Helpers;
using FluxPayAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.QueryHandlerTests.GetTicketsQueryHandlerTests;

public class GetTicketsTestConflict(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterIsNotAdmin()
    {
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());

        var getTicketsQuery = new GetTicketsQuery(alice.Response.Id, 10, DateTime.UtcNow);
        var getTicketsResult = await Fixture.GetTicketsQueryHandler.HandleAsync(getTicketsQuery);

        getTicketsResult.Error.Should().BeOfType<ConflictError>();
    }
}

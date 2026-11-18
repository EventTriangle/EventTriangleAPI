using FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;
using FluxPayAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.QueryHandlerTests.GetSupportTicketsQueryHandlerTests;

public class GetSupportTicketsTestThrowEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var getSupportTicketsQuery = new GetSupportsTicketsQuery(Guid.NewGuid().ToString(), 10, DateTime.UtcNow);
        var getSupportTicketsResult = await Fixture.GetSupportTicketsQueryHandler.HandleAsync(getSupportTicketsQuery);

        getSupportTicketsResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

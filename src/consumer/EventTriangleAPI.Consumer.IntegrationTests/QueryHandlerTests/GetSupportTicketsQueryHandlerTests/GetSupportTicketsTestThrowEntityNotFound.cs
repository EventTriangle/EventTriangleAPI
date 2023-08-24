using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetSupportTicketsQueryHandlerTests;

public class GetSupportTicketsTestThrowEntityNotFound : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var getSupportTicketsQuery = new GetSupportsTicketsQuery(Guid.NewGuid().ToString(), 10, DateTime.UtcNow);

        var getSupportTicketsResult = await GetSupportTicketsQueryHandler.HandleAsync(getSupportTicketsQuery);

        getSupportTicketsResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
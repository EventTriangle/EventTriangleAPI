using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetCreditCardsQueryHandlerTests;

public class GetCreditCardsTestThrowEntityNotFound : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var getCreditCardsQuery = new GetCreditCardsQuery(Guid.NewGuid().ToString());

        var getCreditCardsResult = await GetCreditCardsQueryHandler.HandleAsync(getCreditCardsQuery);

        getCreditCardsResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
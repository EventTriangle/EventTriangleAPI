using FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;
using FluxPayAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.QueryHandlerTests.GetCreditCardsQueryHandlerTests;

public class GetCreditCardsTestThrowEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var getCreditCardsQuery = new GetCreditCardsQuery(Guid.NewGuid().ToString());
        var getCreditCardsResult = await Fixture.GetCreditCardsQueryHandler.HandleAsync(getCreditCardsQuery);

        getCreditCardsResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

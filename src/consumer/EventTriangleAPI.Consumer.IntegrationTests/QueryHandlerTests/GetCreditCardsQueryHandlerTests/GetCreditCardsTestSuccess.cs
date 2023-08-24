using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetCreditCardsQueryHandlerTests;

public class GetCreditCardsTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var firstAddCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(dima.Response.Id);
        var secondAddCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(dima.Response.Id);
        var thirdAddCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(dima.Response.Id);
        
        var firstAddCreditCardResult = await AddCreditCardCommandHandler.HandleAsync(firstAddCreditCardCommand);
        var secondAddCreditCardResult = await AddCreditCardCommandHandler.HandleAsync(secondAddCreditCardCommand);
        var thirdAddCreditCardResult = await AddCreditCardCommandHandler.HandleAsync(thirdAddCreditCardCommand);

        var getCreditCardsQuery = new GetCreditCardsQuery(dima.Response.Id);

        var getCreditCardsResult = await GetCreditCardsQueryHandler.HandleAsync(getCreditCardsQuery);

        var firstCard = getCreditCardsResult.Response.FirstOrDefault(x => x.Id == firstAddCreditCardResult.Response.Id);
        var secondCard = getCreditCardsResult.Response.FirstOrDefault(x => x.Id == secondAddCreditCardResult.Response.Id);
        var thirdCard = getCreditCardsResult.Response.FirstOrDefault(x => x.Id == thirdAddCreditCardResult.Response.Id);

        firstCard.Should().NotBeNull();
        secondCard.Should().NotBeNull();
        thirdCard.Should().NotBeNull();
    }
}
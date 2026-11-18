using FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;
using FluxPayAPI.Consumer.IntegrationTests.Helpers;
using FluxPayAPI.Shared.DTO.Enums;
using FluxPayAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.CommandHandlerTests.ChangeCreditCardCommandHandlerTests;

public class ChangeCreditCardTestThrowEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestCreditCardNotFound()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var changeCreditCardCommand = new ChangeCreditCardCommand(
            Guid.NewGuid(),
            dima.Response.Id,
            "Dima123",
            "1111222233334444",
            "321",
            "12/06",
            PaymentNetwork.Visa);
        var changeCreditCardResult = await Fixture.ChangeCreditCardCommandHandler.HandleAsync(changeCreditCardCommand);

        changeCreditCardResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.ChangeCreditCardCommandHandlerTests;

public class ChangeCreditCardTestThrowEntityNotFound : IntegrationTestBase
{
    [Fact]
    public async Task TestCreditCardNotFound()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserDimaCommand());

        var changeCreditCardCommand = new ChangeCreditCardCommand(
            Guid.NewGuid(),
            dima.Response.Id,
            "Dima123",
            "1111222233334444",
            "321",
            "12/06",
            PaymentNetwork.Visa);

        var changeCreditCardResult = await ChangeCreditCardCommandHandler.HandleAsync(changeCreditCardCommand);

        changeCreditCardResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.AddCreditCardCommandHandlerTests;

public class AddCreditCardTestThrowBadRequest : IntegrationTestBase
{
    [Fact]
    public async Task TestAddingTheSameCreditCardAgain()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserDimaCommand());

        var addCreditCardCommand = new AddCreditCardCommand(
            dima.Response.Id,
            "Dima",
            "1234567890123456",
            "123",
            "04/12",
            PaymentNetwork.MasterCard);

         await AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);

         var secondAddCreditCardResult = await AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);

         secondAddCreditCardResult.Error.Should().BeOfType<BadRequestError>();
    }
}
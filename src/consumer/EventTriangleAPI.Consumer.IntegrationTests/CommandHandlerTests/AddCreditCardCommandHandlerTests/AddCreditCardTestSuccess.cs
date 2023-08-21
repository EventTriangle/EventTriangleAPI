using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.AddCreditCardCommandHandlerTests;

public class AddCreditCardTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserDimaCommand());

        var addCreditCardCommand = new AddCreditCardCommand(
            dima.Response.Id,
            "Dima",
            "1234567890123456",
            "123",
            "04/12",
            PaymentNetwork.MasterCard);

        var addCreditCardResult = await AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);

        var creditCard = await DatabaseContextFixture.CreditCardEntities
            .FirstOrDefaultAsync(x => x.Id == addCreditCardResult.Response.Id);

        creditCard.UserId.Should().Be(dima.Response.Id);
        creditCard.HolderName.Should().Be(addCreditCardCommand.HolderName);
        creditCard.CardNumber.Should().Be(addCreditCardCommand.CardNumber);
        creditCard.Cvv.Should().Be(addCreditCardCommand.Cvv);
        creditCard.Expiration.Should().Be(addCreditCardCommand.Expiration);
        creditCard.PaymentNetwork.Should().Be(addCreditCardCommand.PaymentNetwork);
    }
}
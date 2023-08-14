using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandTests.ChangeCreditCardCommandHandlerTests;

public class ChangeCreditCardTestSuccess : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
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

        var changeCreditCardCommand = new ChangeCreditCardCommand(
            addCreditCardResult.Response.Id,
            dima.Response.Id,
            "Dima123",
            "1111222233334444",
            "321",
            "12/06",
            PaymentNetwork.Visa);

        await ChangeCreditCardCommandHandler.HandleAsync(changeCreditCardCommand);

        var creditCard = await DatabaseContextFixture.CreditCardEntities
            .FirstOrDefaultAsync(x => x.Id == addCreditCardResult.Response.Id);
        
        creditCard.UserId.Should().Be(dima.Response.Id);
        creditCard.HolderName.Should().Be(changeCreditCardCommand.HolderName);
        creditCard.CardNumber.Should().Be(changeCreditCardCommand.CardNumber);
        creditCard.Cvv.Should().Be(changeCreditCardCommand.Cvv);
        creditCard.Expiration.Should().Be(changeCreditCardCommand.Expiration);
        creditCard.PaymentNetwork.Should().Be(changeCreditCardCommand.PaymentNetwork);
    }
}
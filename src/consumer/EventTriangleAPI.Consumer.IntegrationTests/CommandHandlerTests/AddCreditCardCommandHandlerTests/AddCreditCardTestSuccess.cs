using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.AddCreditCardCommandHandlerTests;

public class AddCreditCardTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var addCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(dima.Response.Id);
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
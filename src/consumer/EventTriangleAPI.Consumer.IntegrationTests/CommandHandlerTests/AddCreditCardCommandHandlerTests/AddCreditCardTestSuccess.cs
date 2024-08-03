using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.AddCreditCardCommandHandlerTests;

public class AddCreditCardTestSuccess(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestSuccess()
    {
        // Arrange
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var addCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(dima.Response.Id);

        // Act
        var addCreditCardResult = await Fixture.AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);

        // Assert
        var creditCard = await Fixture.DatabaseContextFixture.CreditCardEntities
            .FirstOrDefaultAsync(x => x.Id == addCreditCardResult.Response.Id);
        creditCard.UserId.Should().Be(dima.Response.Id);
        creditCard.HolderName.Should().Be(addCreditCardCommand.HolderName);
        creditCard.CardNumber.Should().Be(addCreditCardCommand.CardNumber);
        creditCard.Cvv.Should().Be(addCreditCardCommand.Cvv);
        creditCard.Expiration.Should().Be(addCreditCardCommand.Expiration);
        creditCard.PaymentNetwork.Should().Be(addCreditCardCommand.PaymentNetwork);
    }
}

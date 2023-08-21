using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.AddCreditCardCommandHandlerTests;

public class AddCreditCardTestThrowEntityNotFound : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var addCreditCardCommand = new AddCreditCardCommand(
            Guid.NewGuid().ToString(),
            "Dima",
            "1234567890123456",
            "123",
            "04/12",
            PaymentNetwork.MasterCard);

        var addCreditCardResult = await AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);

        addCreditCardResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.AddCreditCardCommandHandlerTests;

public class AddCreditCardTestThrowBadRequest : IntegrationTestBase
{
    [Fact]
    public async Task TestAddingTheSameCreditCardAgain()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var addCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(dima.Response.Id);
        await AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);
        var secondAddCreditCardResult = await AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);

         secondAddCreditCardResult.Error.Should().BeOfType<BadRequestError>();
    }
}
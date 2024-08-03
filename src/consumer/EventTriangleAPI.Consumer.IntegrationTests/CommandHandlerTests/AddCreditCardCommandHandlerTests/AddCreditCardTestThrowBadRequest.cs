using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.AddCreditCardCommandHandlerTests;

public class AddCreditCardTestThrowBadRequest(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestAddingTheSameCreditCardAgain()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var addCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(dima.Response.Id);
        await Fixture.AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);
        var secondAddCreditCardResult = await Fixture.AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);

         secondAddCreditCardResult.Error.Should().BeOfType<BadRequestError>();
    }
}

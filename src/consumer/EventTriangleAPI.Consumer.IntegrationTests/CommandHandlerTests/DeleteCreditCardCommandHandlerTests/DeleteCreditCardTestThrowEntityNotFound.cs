using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.DeleteCreditCardCommandHandlerTests;

public class DeleteCreditCardTestThrowEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var addCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(dima.Response.Id);
        var addCreditCardResult = await Fixture.AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);

        var deleteCreditCardByNobodyCommand =
            new DeleteCreditCardCommand(Guid.NewGuid().ToString(), addCreditCardResult.Response.Id);
        var deleteCreditCardByNobodyResult =
            await Fixture.DeleteCreditCardCommandHandler.HandleAsync(deleteCreditCardByNobodyCommand);

        deleteCreditCardByNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }

    [Fact]
    public async Task TestCreditCardNotFound()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var deleteCreditCardForNonExistentCardCommand =
            new DeleteCreditCardCommand(dima.Response.Id, Guid.NewGuid());
        var deleteCreditCardForNonExistentCardResult =
            await Fixture.DeleteCreditCardCommandHandler.HandleAsync(deleteCreditCardForNonExistentCardCommand);

        deleteCreditCardForNonExistentCardResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

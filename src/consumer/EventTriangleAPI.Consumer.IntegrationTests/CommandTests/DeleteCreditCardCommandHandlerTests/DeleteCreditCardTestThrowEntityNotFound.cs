using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandTests.DeleteCreditCardCommandHandlerTests;

public class DeleteCreditCardTestThrowEntityNotFound : IntegrationTestBase, IIntegrationTest
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

        var deleteCreditCardByNobodyCommand = 
            new DeleteCreditCardCommand(Guid.NewGuid().ToString(), addCreditCardResult.Response.Id);
        var deleteCreditCardForNonExistentCardCommand =
            new DeleteCreditCardCommand(dima.Response.Id, Guid.NewGuid());

        var deleteCreditCardByNobodyResult = 
            await DeleteCreditCardCommandHandler.HandleAsync(deleteCreditCardByNobodyCommand);
        var deleteCreditCardForNonExistentCardResult = 
            await DeleteCreditCardCommandHandler.HandleAsync(deleteCreditCardForNonExistentCardCommand);

        deleteCreditCardByNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
        deleteCreditCardForNonExistentCardResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
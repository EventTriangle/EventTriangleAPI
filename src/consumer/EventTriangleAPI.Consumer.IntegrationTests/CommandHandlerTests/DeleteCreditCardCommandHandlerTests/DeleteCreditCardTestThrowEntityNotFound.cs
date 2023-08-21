using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.DeleteCreditCardCommandHandlerTests;

public class DeleteCreditCardTestThrowEntityNotFound : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterNotFound()
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

        var deleteCreditCardByNobodyResult = 
            await DeleteCreditCardCommandHandler.HandleAsync(deleteCreditCardByNobodyCommand);

        deleteCreditCardByNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
    
    [Fact]
    public async Task TestCreditCardNotFound()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserDimaCommand());

        var deleteCreditCardForNonExistentCardCommand =
        new DeleteCreditCardCommand(dima.Response.Id, Guid.NewGuid());

        var deleteCreditCardForNonExistentCardResult = 
        await DeleteCreditCardCommandHandler.HandleAsync(deleteCreditCardForNonExistentCardCommand);

        deleteCreditCardForNonExistentCardResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
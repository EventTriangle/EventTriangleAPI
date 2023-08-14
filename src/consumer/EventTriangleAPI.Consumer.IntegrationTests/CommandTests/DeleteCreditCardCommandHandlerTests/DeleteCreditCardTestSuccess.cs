using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandTests.DeleteCreditCardCommandHandlerTests;

public class DeleteCreditCardTestSuccess : IntegrationTestBase, IIntegrationTest
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

        var isCardExisting = await DatabaseContextFixture.CreditCardEntities
            .AnyAsync(x => x.Id == addCreditCardResult.Response.Id);
                    
        var deleteCreditCard = new DeleteCreditCardCommand(dima.Response.Id, addCreditCardResult.Response.Id);

        await DeleteCreditCardCommandHandler.HandleAsync(deleteCreditCard);
        
        var isCardExistingAfterDeleting = await DatabaseContextFixture.CreditCardEntities
            .AnyAsync(x => x.Id == addCreditCardResult.Response.Id);

        isCardExisting.Should().BeTrue();
        isCardExistingAfterDeleting.Should().BeFalse();
    }
}
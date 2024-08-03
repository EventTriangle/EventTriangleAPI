using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.DeleteCreditCardCommandHandlerTests;

public class DeleteCreditCardTestSuccess(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var addCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(dima.Response.Id);
        var addCreditCardResult = await Fixture.AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);
        var isCardExisting = await Fixture.DatabaseContextFixture.CreditCardEntities
            .AnyAsync(x => x.Id == addCreditCardResult.Response.Id);

        var deleteCreditCard = new DeleteCreditCardCommand(dima.Response.Id, addCreditCardResult.Response.Id);
        await Fixture.DeleteCreditCardCommandHandler.HandleAsync(deleteCreditCard);

        var isCardExistingAfterDeleting = await Fixture.DatabaseContextFixture.CreditCardEntities
            .AnyAsync(x => x.Id == addCreditCardResult.Response.Id);
        isCardExisting.Should().BeTrue();
        isCardExistingAfterDeleting.Should().BeFalse();
    }
}

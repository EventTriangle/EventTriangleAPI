using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class AttachCreditCardToAccountCommandHandlerTest(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task Test()
    {
        var command = new AttachCreditCardToAccountCommand(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            new string('1', 16),
            "12/12",
            "123",
            PaymentNetwork.MasterCard);

        var result = await Fixture.AttachCreditCardToAccountCommandHandler.HandleAsync(command);

        var creditCardAttachedEvent =
            await Fixture.DatabaseContextFixture.CreditCardAddedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        creditCardAttachedEvent.Should().NotBeNull();
    }
}

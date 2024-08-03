using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class EditCreditCardCommandHandlerTest(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task Test()
    {
        var command = new EditCreditCardCommand(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(),
            Guid.NewGuid().ToString(),
            new string('1', 16),
            "12/12",
            "123",
            PaymentNetwork.MasterCard);

        var result = await Fixture.EditCreditCardCommandHandler.HandleAsync(command);

        var creditCardChangedEvent =
            await Fixture.DatabaseContextFixture.CreditCardChangedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        creditCardChangedEvent.Should().NotBeNull();
    }
}

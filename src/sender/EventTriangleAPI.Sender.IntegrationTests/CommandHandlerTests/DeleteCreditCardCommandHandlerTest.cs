using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class DeleteCreditCardCommandHandlerTest(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task Test()
    {
        var command = new DeleteCreditCardCommand(Guid.NewGuid().ToString(), Guid.NewGuid());

        var result = await Fixture.DeleteCreditCardCommandHandler.HandleAsync(command);

        var creditCardDeletedEvent =
            await Fixture.DatabaseContextFixture.CreditCardDeletedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        creditCardDeletedEvent.Should().NotBeNull();
    }
}

using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.Application.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class AttachCreditCardToAccountCommandHandlerTest : IntegrationTestBase, IIntegrationTest
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
        
        var result = await AttachCreditCardToAccountCommandHandler.HandleAsync(command);

        var addContactEvent = 
            await DatabaseContextFixture.CreditCardAddedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        addContactEvent.Should().NotBeNull();
    }
}
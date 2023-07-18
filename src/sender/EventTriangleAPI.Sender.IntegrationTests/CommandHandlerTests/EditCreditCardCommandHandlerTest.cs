using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.Application.Enums;
using EventTriangleAPI.Shared.DTO.Commands;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class EditCreditCardCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var body = new EditCreditCardBody(
            Guid.NewGuid().ToString(),
            Guid.NewGuid(), 
            Guid.NewGuid().ToString(),
            new string('1', 16),
            "12/12",
            "123",
            PaymentNetwork.MasterCard);
        var command = new Command<EditCreditCardBody>(body);
        
        var result = await EditCreditCardCommandHandler.HandleAsync(command);

        var addContactEvent = 
            await DatabaseContextFixture.CreditCardChangedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        addContactEvent.Should().NotBeNull();
    }
}
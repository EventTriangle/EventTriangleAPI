using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.DTO.Commands;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class DeleteCreditCardCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var body = new DeleteCreditCardBody(Guid.NewGuid().ToString(), Guid.NewGuid());
        var command = new Command<DeleteCreditCardBody>(body);
        
        var result = await DeleteCreditCardCommandHandler.HandleAsync(command);

        var addContactEvent = 
            await DatabaseContextFixture.CreditCardDeletedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        addContactEvent.Should().NotBeNull();
    }
}
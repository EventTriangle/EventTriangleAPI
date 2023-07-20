using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class DeleteCreditCardCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var command = new DeleteCreditCardCommand(Guid.NewGuid().ToString(), Guid.NewGuid());
        
        var result = await DeleteCreditCardCommandHandler.HandleAsync(command);

        var addContactEvent = 
            await DatabaseContextFixture.CreditCardDeletedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        addContactEvent.Should().NotBeNull();
    }
}
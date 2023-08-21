using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.CreateContactCommandHandlerTests;

public class CreateContactTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserAliceCommand());

        var createContactForDimaCommand = new CreateContactCommand(dima.Response.Id, alice.Response.Id);
        var createContactForAliceCommand = new CreateContactCommand(alice.Response.Id, dima.Response.Id);

        await CreateContactCommandHandler.HandleAsync(createContactForDimaCommand);
        await CreateContactCommandHandler.HandleAsync(createContactForAliceCommand);
        
        var dimaContact = await DatabaseContextFixture.ContactEntities
            .FirstOrDefaultAsync(x => x.UserId == dima.Response.Id && x.ContactId == alice.Response.Id);

        var aliceContact = await DatabaseContextFixture.ContactEntities
            .FirstOrDefaultAsync(x => x.UserId == alice.Response.Id && x.ContactId == dima.Response.Id);
        
        dimaContact.UserId.Should().Be(dima.Response.Id);
        dimaContact.ContactId.Should().Be(alice.Response.Id);
        
        aliceContact.UserId.Should().Be(alice.Response.Id);
        aliceContact.ContactId.Should().Be(dima.Response.Id);
    }
}
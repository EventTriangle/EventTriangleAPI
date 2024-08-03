using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.CreateContactCommandHandlerTests;

public class CreateContactTestSuccess(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());

        var createContactForDimaCommand = new CreateContactCommand(dima.Response.Id, alice.Response.Id);
        var createContactForAliceCommand = new CreateContactCommand(alice.Response.Id, dima.Response.Id);
        await Fixture.CreateContactCommandHandler.HandleAsync(createContactForDimaCommand);
        await Fixture.CreateContactCommandHandler.HandleAsync(createContactForAliceCommand);

        var dimaContact = await Fixture.DatabaseContextFixture.ContactEntities
            .FirstOrDefaultAsync(x => x.UserId == dima.Response.Id && x.ContactId == alice.Response.Id);
        var aliceContact = await Fixture.DatabaseContextFixture.ContactEntities
            .FirstOrDefaultAsync(x => x.UserId == alice.Response.Id && x.ContactId == dima.Response.Id);
        dimaContact.UserId.Should().Be(dima.Response.Id);
        dimaContact.ContactId.Should().Be(alice.Response.Id);
        aliceContact.UserId.Should().Be(alice.Response.Id);
        aliceContact.ContactId.Should().Be(dima.Response.Id);
    }
}

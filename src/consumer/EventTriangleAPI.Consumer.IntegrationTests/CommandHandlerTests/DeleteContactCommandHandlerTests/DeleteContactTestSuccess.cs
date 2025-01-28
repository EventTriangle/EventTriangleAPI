using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.DeleteContactCommandHandlerTests;

public class DeleteContactTestSuccess(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var createContactForDimaCommand = new CreateContactCommand(dima.Response.Id, alice.Response.Id);
        await Fixture.CreateContactCommandHandler.HandleAsync(createContactForDimaCommand);
        var isContactExisting = await Fixture.DatabaseContextFixture.ContactEntities
            .AnyAsync(x => x.UserId == dima.Response.Id && x.ContactId == alice.Response.Id);

        var deleteContactCommand = new DeleteContactCommand(dima.Response.Id, alice.Response.Id);
        await Fixture.DeleteContactCommandHandler.HandleAsync(deleteContactCommand);

        var isContactExistingAfterDeleting = await Fixture.DatabaseContextFixture.ContactEntities
            .AnyAsync(x => x.UserId == dima.Response.Id && x.ContactId == alice.Response.Id);
        isContactExisting.Should().BeTrue();
        isContactExistingAfterDeleting.Should().BeFalse();
    }
}

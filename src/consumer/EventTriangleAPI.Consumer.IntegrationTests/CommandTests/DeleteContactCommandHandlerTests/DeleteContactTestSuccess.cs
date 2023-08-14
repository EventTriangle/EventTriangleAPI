using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandTests.DeleteContactCommandHandlerTests;

public class DeleteContactTestSuccess : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserAliceCommand());

        var createContactForDimaCommand = new CreateContactCommand(dima.Response.Id, alice.Response.Id);

        await CreateContactCommandHandler.HandleAsync(createContactForDimaCommand);

        var isContactExisting = await DatabaseContextFixture.ContactEntities
            .AnyAsync(x => x.UserId == dima.Response.Id && x.ContactId == alice.Response.Id);
        
        var deleteContactCommand = new DeleteContactCommand(dima.Response.Id, alice.Response.Id);

        await DeleteContactCommandHandler.HandleAsync(deleteContactCommand);
        
        var isContactExistingAfterDeleting = await DatabaseContextFixture.ContactEntities
            .AnyAsync(x => x.UserId == dima.Response.Id && x.ContactId == alice.Response.Id);

        isContactExisting.Should().BeTrue();
        isContactExistingAfterDeleting.Should().BeFalse();
    }
}
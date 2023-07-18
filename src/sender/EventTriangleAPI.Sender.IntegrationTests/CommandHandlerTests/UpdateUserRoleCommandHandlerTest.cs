using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.Application.Enums;
using EventTriangleAPI.Shared.DTO.Commands;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class UpdateUserRoleCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var body = new UpdateUserRoleBody(Guid.NewGuid().ToString(), UserRole.Admin);
        var command = new Command<UpdateUserRoleBody>(body);
        
        var result = await UpdateUserRoleCommandHandler.HandleAsync(command);

        var addContactEvent = 
            await DatabaseContextFixture.UserRoleUpdatedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        addContactEvent.Should().NotBeNull();
    }
}
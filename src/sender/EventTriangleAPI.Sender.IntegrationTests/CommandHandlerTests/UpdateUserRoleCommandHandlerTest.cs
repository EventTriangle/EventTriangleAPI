using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class UpdateUserRoleCommandHandlerTest : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var command = new UpdateUserRoleCommand(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), UserRole.Admin);
        
        var result = await UpdateUserRoleCommandHandler.HandleAsync(command);

        var userRoleUpdatedEvent = 
            await DatabaseContextFixture.UserRoleUpdatedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        userRoleUpdatedEvent.Should().NotBeNull();
    }
}
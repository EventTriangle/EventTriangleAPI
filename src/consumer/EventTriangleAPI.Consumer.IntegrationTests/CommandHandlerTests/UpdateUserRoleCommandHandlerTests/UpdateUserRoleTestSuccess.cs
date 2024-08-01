using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.UpdateUserRoleCommandHandlerTests;

public class UpdateUserRoleTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());

        var updateUserRoleCommand = new UpdateUserRoleCommand(dima.Response.Id, alice.Response.Id, UserRole.Admin);
        await UpdateUserRoleCommandHandler.HandleAsync(updateUserRoleCommand);

        var user = await DatabaseContextFixture.UserEntities.FirstAsync(x => x.Id == alice.Response.Id);
        user.UserRole.Should().Be(UserRole.Admin);
    }
}
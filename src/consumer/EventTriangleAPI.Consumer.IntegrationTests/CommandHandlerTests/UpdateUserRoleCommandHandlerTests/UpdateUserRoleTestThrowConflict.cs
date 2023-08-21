using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.UpdateUserRoleCommandHandlerTests;

public class UpdateUserRoleTestThrowConflict : IntegrationTestBase
{
    [Fact]
    public async void TestConflict()
    {
        var bob = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserBobCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserAliceCommand());

        var updateUserRoleCommand = new UpdateUserRoleCommand(bob.Response.Id, alice.Response.Id, UserRole.Admin);

        var updateUserRoleResult =  await UpdateUserRoleCommandHandler.HandleAsync(updateUserRoleCommand);

        updateUserRoleResult.Error.Should().BeOfType<ConflictError>();
    }
}
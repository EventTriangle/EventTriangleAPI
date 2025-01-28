using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.UpdateUserRoleCommandHandlerTests;

public class UpdateUserRoleTestThrowConflict(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestConflict()
    {
        var bob = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());

        var updateUserRoleCommand = new UpdateUserRoleCommand(bob.Response.Id, alice.Response.Id, UserRole.Admin);
        var updateUserRoleResult =  await Fixture.UpdateUserRoleCommandHandler.HandleAsync(updateUserRoleCommand);

        updateUserRoleResult.Error.Should().BeOfType<ConflictError>();
    }
}

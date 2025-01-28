using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.SuspendUserCommandHandlerTests;

public class SuspendUserTestThrowConflict(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterIsNotAdmin()
    {
        var bob = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());

        var suspendUserCommand = new SuspendUserCommand(bob.Response.Id, alice.Response.Id);
        var suspendUserResult = await Fixture.SuspendUserCommandHandler.HandleAsync(suspendUserCommand);

        suspendUserResult.Error.Should().BeOfType<ConflictError>();
    }

    [Fact]
    public async Task TestUserIsAdmin()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var updateUserRoleCommand = new UpdateUserRoleCommand(dima.Response.Id, alice.Response.Id, UserRole.Admin);
        await Fixture.UpdateUserRoleCommandHandler.HandleAsync(updateUserRoleCommand);

        var suspendUserCommand = new SuspendUserCommand(dima.Response.Id, alice.Response.Id);
        var suspendUserResult = await Fixture.SuspendUserCommandHandler.HandleAsync(suspendUserCommand);

        suspendUserResult.Error.Should().BeOfType<ConflictError>();
    }
}

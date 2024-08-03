using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.UpdateUserRoleCommandHandlerTests;

public class UpdateUserRoleTestThrowEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());

        var updateUserRoleCommand = new UpdateUserRoleCommand(Guid.NewGuid().ToString(), alice.Response.Id, UserRole.Admin);
        var updateUserRoleResult = await Fixture.UpdateUserRoleCommandHandler.HandleAsync(updateUserRoleCommand);

        updateUserRoleResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }

    [Fact]
    public async Task TestUserNotFound()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var updateUserRoleCommand = new UpdateUserRoleCommand(dima.Response.Id, Guid.NewGuid().ToString(), UserRole.Admin);
        var updateUserRoleResult = await Fixture.UpdateUserRoleCommandHandler.HandleAsync(updateUserRoleCommand);

        updateUserRoleResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

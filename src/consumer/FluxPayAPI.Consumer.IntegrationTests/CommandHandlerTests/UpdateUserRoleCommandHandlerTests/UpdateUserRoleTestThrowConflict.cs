using FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;
using FluxPayAPI.Consumer.IntegrationTests.Helpers;
using FluxPayAPI.Shared.DTO.Enums;
using FluxPayAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.CommandHandlerTests.UpdateUserRoleCommandHandlerTests;

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

using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.SuspendUserCommandHandlerTests;

public class SuspendUserTestThrowConflict : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterIsNotAdmin()
    {
        var bob = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserBobCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserAliceCommand());

        var suspendUserCommand = new SuspendUserCommand(bob.Response.Id, alice.Response.Id);

        var suspendUserResult = await SuspendUserCommandHandler.HandleAsync(suspendUserCommand);
        
        suspendUserResult.Error.Should().BeOfType<ConflictError>();
    }
}
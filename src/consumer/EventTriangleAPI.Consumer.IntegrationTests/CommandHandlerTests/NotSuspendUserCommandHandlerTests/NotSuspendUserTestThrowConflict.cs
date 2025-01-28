using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.NotSuspendUserCommandHandlerTests;

public class NotSuspendUserTestThrowConflict(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterIsNotAdmin()
    {
        var bob = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());

        var notSuspendUserCommand = new NotSuspendUserCommand(bob.Response.Id, alice.Response.Id);
        var notSuspendUserResult = await Fixture.NotSuspendUserCommandHandler.HandleAsync(notSuspendUserCommand);

        notSuspendUserResult.Error.Should().BeOfType<ConflictError>();
    }
}

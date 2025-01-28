using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.SuspendUserCommandHandlerTests;

public class SuspendUserTestThrowEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());

        var suspendUserCommand = new SuspendUserCommand(Guid.NewGuid().ToString(), alice.Response.Id);
        var suspendUserResult = await Fixture.SuspendUserCommandHandler.HandleAsync(suspendUserCommand);

        suspendUserResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }

    [Fact]
    public async Task TestUserNotFound()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var suspendUserCommand = new SuspendUserCommand(dima.Response.Id, Guid.NewGuid().ToString());
        var suspendUserResult = await Fixture.SuspendUserCommandHandler.HandleAsync(suspendUserCommand);

        suspendUserResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

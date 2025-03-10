using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.NotSuspendUserCommandHandlerTests;

public class NotSuspendUserTestThrowEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var suspendUserCommand = new SuspendUserCommand(dima.Response.Id, alice.Response.Id);
        await Fixture.SuspendUserCommandHandler.HandleAsync(suspendUserCommand);
        var aliceAfterSuspending = await Fixture.DatabaseContextFixture.UserEntities
            .FirstOrDefaultAsync(x => x.Id == alice.Response.Id);

        var notSuspendUserCommand = new NotSuspendUserCommand(Guid.NewGuid().ToString(), alice.Response.Id);
        var notSuspendUserResult = await Fixture.NotSuspendUserCommandHandler.HandleAsync(notSuspendUserCommand);

        aliceAfterSuspending.UserStatus.Should().Be(UserStatus.Suspended);
        notSuspendUserResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }

    [Fact]
    public async Task TestUserNotFound()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var notSuspendUserCommand = new NotSuspendUserCommand(dima.Response.Id, Guid.NewGuid().ToString());
        var notSuspendUserResult = await Fixture.NotSuspendUserCommandHandler.HandleAsync(notSuspendUserCommand);

        notSuspendUserResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

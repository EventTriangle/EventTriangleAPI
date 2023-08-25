using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.NotSuspendUserCommandHandlerTests;

public class NotSuspendUserTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var suspendUserCommand = new SuspendUserCommand(dima.Response.Id, alice.Response.Id);
        await SuspendUserCommandHandler.HandleAsync(suspendUserCommand);
        var aliceAfterSuspending = await DatabaseContextFixture.UserEntities
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == alice.Response.Id);
        
        var notSuspendUserCommand = new NotSuspendUserCommand(dima.Response.Id, alice.Response.Id);
        await NotSuspendUserCommandHandler.HandleAsync(notSuspendUserCommand);
        
        var aliceAfterStopSuspending = await DatabaseContextFixture.UserEntities
            .FirstOrDefaultAsync(x => x.Id == alice.Response.Id);
        aliceAfterSuspending.UserStatus.Should().Be(UserStatus.Suspended);
        aliceAfterStopSuspending.UserStatus.Should().Be(UserStatus.Active);
    }
}
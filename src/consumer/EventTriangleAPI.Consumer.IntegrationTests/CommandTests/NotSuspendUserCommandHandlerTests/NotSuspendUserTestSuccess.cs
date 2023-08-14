using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandTests.NotSuspendUserCommandHandlerTests;

public class NotSuspendUserTestSuccess : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var alice = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserAliceCommand());

        var suspendUserCommand = new SuspendUserCommand(alice.Response.Id);

        await SuspendUserCommandHandler.HandleAsync(suspendUserCommand);

        var aliceAfterSuspending = await DatabaseContextFixture.UserEntities
            .FirstOrDefaultAsync(x => x.Id == alice.Response.Id);

        aliceAfterSuspending.UserStatus.Should().Be(UserStatus.Suspended);
        
        var notSuspendUserCommand = new NotSuspendUserCommand(alice.Response.Id);

        await NotSuspendUserCommandHandler.HandleAsync(notSuspendUserCommand);
        
        var aliceAfterStopSuspending = await DatabaseContextFixture.UserEntities
            .FirstOrDefaultAsync(x => x.Id == alice.Response.Id);

        aliceAfterStopSuspending.UserStatus.Should().Be(UserStatus.Active);
    }
}
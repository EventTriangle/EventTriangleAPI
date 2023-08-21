using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.SuspendUserCommandHandlerTests;

public class SuspendUserTestSuccess : IntegrationTestBase
{
    [Fact]
    public async void TestSuccess()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserAliceCommand());
        
        var suspendUserCommand = new SuspendUserCommand(dima.Response.Id, alice.Response.Id);
        
        await SuspendUserCommandHandler.HandleAsync(suspendUserCommand);
        
        var aliceAfterSuspending = await DatabaseContextFixture.UserEntities
            .FirstOrDefaultAsync(x => x.Id == alice.Response.Id);
        
        aliceAfterSuspending.UserStatus.Should().Be(UserStatus.Suspended);
    }
}
using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.NotSuspendUserCommandHandlerTests;

public class NotSuspendUserTestThrowEntityNotFound : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserAliceCommand());
        
        var suspendUserCommand = new SuspendUserCommand(dima.Response.Id, alice.Response.Id);
        
        await SuspendUserCommandHandler.HandleAsync(suspendUserCommand);
        
        var aliceAfterSuspending = await DatabaseContextFixture.UserEntities
            .FirstOrDefaultAsync(x => x.Id == alice.Response.Id);
        
        aliceAfterSuspending.UserStatus.Should().Be(UserStatus.Suspended);
        
        var notSuspendUserCommand = new NotSuspendUserCommand(Guid.NewGuid().ToString(), alice.Response.Id);

        var notSuspendUserResult = await NotSuspendUserCommandHandler.HandleAsync(notSuspendUserCommand);

        notSuspendUserResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
    
    [Fact]
    public async Task TestUserNotFound()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserDimaCommand());
        
        var notSuspendUserCommand = new NotSuspendUserCommand(dima.Response.Id, Guid.NewGuid().ToString());

        var notSuspendUserResult = await NotSuspendUserCommandHandler.HandleAsync(notSuspendUserCommand);

        notSuspendUserResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
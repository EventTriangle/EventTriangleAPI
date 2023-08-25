using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.SuspendUserCommandHandlerTests;

public class SuspendUserTestThrowEntityNotFound : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var alice = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        
        var suspendUserCommand = new SuspendUserCommand(Guid.NewGuid().ToString(), alice.Response.Id);
        var suspendUserResult = await SuspendUserCommandHandler.HandleAsync(suspendUserCommand);

        suspendUserResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
    
    [Fact]
    public async Task TestUserNotFound()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        
        var suspendUserCommand = new SuspendUserCommand(dima.Response.Id, Guid.NewGuid().ToString());
        var suspendUserResult = await SuspendUserCommandHandler.HandleAsync(suspendUserCommand);

        suspendUserResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
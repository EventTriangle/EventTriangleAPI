using FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;
using FluxPayAPI.Consumer.IntegrationTests.Helpers;
using FluxPayAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.CommandHandlerTests.SuspendUserCommandHandlerTests;

public class SuspendUserTestSuccess(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());

        var suspendUserCommand = new SuspendUserCommand(dima.Response.Id, alice.Response.Id);
        await Fixture.SuspendUserCommandHandler.HandleAsync(suspendUserCommand);

        var aliceAfterSuspending = await Fixture.DatabaseContextFixture.UserEntities
            .FirstOrDefaultAsync(x => x.Id == alice.Response.Id);
        aliceAfterSuspending.UserStatus.Should().Be(UserStatus.Suspended);
    }
}

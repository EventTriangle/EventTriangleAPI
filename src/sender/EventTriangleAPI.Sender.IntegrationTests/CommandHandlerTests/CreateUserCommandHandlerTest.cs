using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests.CommandHandlerTests;

public class CreateUserCommandHandlerTest(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task Test()
    {
        var command = new CreateUserCommand(Guid.NewGuid().ToString(), "user@gmail.com", UserRole.User, UserStatus.Active);

        var result = await Fixture.CreateUserCommandHandler.HandleAsync(command);

        var userCreatedEvent =
            await Fixture.DatabaseContextFixture.UserCreatedEvents.FirstOrDefaultAsync(x => x.Id == result.Response.Id);

        userCreatedEvent.Should().NotBeNull();
    }
}

using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.CreateUserCommandHandlerTests;

public class CreateUserTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var createUserCommand = CreateUserCommandHelper.CreateUserDimaCommand();
        await CreateUserCommandHandler.HandleAsync(createUserCommand);

        var user = await DatabaseContextFixture.UserEntities.FirstOrDefaultAsync(x => x.Id == createUserCommand.UserId);
        user.Id.Should().Be(createUserCommand.UserId);
        user.Email.Should().Be(createUserCommand.Email);
        user.UserRole.Should().Be(createUserCommand.UserRole);
        user.UserStatus.Should().Be(createUserCommand.UserStatus);
    }
}
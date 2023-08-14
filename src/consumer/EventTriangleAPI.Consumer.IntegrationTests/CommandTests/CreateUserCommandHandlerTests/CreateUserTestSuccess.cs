using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandTests.CreateUserCommandHandlerTests;

public class CreateUserTestSuccess : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var createUserCommand = CommandHelper.CreateUserDimaCommand();

        await CreateUserCommandHandler.HandleAsync(createUserCommand);

        var user = await DatabaseContextFixture.UserEntities.FirstOrDefaultAsync(x => x.Id == createUserCommand.UserId);

        user.Id.Should().Be(createUserCommand.UserId);
        user.Email.Should().Be(createUserCommand.Email);
        user.UserRole.Should().Be(createUserCommand.UserRole);
        user.UserStatus.Should().Be(createUserCommand.UserStatus);
    }
}
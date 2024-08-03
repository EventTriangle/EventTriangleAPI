using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.CreateUserCommandHandlerTests;

public class CreateUserTestThrowEntityExists(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestUserAlreadyExists()
    {
        var createUserCommand = CreateUserCommandHelper.CreateUserDimaCommand();
        await Fixture.CreateUserCommandHandler.HandleAsync(createUserCommand);

        var secondCreateUserResult = await Fixture.CreateUserCommandHandler.HandleAsync(createUserCommand);
        secondCreateUserResult.Error.Should().BeOfType<DbEntityExistsError>();
    }
}

using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.CreateUserCommandHandlerTests;

public class CreateUserTestThrowEntityExists : IntegrationTestBase
{
    [Fact]
    public async Task TestUserAlreadyExists()
    {
        var createUserCommand = CreateUserCommandHelper.CreateUserDimaCommand();
        await CreateUserCommandHandler.HandleAsync(createUserCommand);
        
        var secondCreateUserResult = await CreateUserCommandHandler.HandleAsync(createUserCommand);
        secondCreateUserResult.Error.Should().BeOfType<DbEntityExistsError>();
    }
}
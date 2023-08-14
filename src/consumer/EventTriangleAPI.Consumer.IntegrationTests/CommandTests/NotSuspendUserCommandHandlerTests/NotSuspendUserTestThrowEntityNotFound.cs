using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandTests.NotSuspendUserCommandHandlerTests;

public class NotSuspendUserTestThrowEntityNotFound : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var notSuspendUserCommand = new NotSuspendUserCommand(Guid.NewGuid().ToString());

        var notSuspendUserResult = await NotSuspendUserCommandHandler.HandleAsync(notSuspendUserCommand);

        notSuspendUserResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
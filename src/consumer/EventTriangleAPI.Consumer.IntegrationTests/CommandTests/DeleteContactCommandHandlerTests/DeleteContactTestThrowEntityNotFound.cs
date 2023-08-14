using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandTests.DeleteContactCommandHandlerTests;

public class DeleteContactTestThrowEntityNotFound : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserAliceCommand());

        var deleteContactByNobodyCommand = new DeleteContactCommand(Guid.NewGuid().ToString(), alice.Response.Id);
        var deleteContactWithNobodyCommand = new DeleteContactCommand(dima.Response.Id, Guid.NewGuid().ToString());

        var deleteContactByNobodyResult = await DeleteContactCommandHandler.HandleAsync(deleteContactByNobodyCommand);
        var deleteContactWithNobodyResult = await DeleteContactCommandHandler.HandleAsync(deleteContactWithNobodyCommand);

        deleteContactByNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
        deleteContactWithNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
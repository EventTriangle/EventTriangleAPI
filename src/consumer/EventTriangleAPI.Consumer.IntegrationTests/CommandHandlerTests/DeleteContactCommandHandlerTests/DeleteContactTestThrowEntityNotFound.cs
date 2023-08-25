using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.DeleteContactCommandHandlerTests;

public class DeleteContactTestThrowEntityNotFound : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var alice = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());

        var deleteContactByNobodyCommand = new DeleteContactCommand(Guid.NewGuid().ToString(), alice.Response.Id);
        var deleteContactByNobodyResult = await DeleteContactCommandHandler.HandleAsync(deleteContactByNobodyCommand);

        deleteContactByNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
    
    [Fact]
    public async Task TestContactNotFound()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var deleteContactWithNobodyCommand = new DeleteContactCommand(dima.Response.Id, Guid.NewGuid().ToString());
        var deleteContactWithNobodyResult = await DeleteContactCommandHandler.HandleAsync(deleteContactWithNobodyCommand);

        deleteContactWithNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
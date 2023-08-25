using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.CreateContactCommandHandlerTests;

public class CreateContactTestThrowEntityNotFound : IntegrationTestBase
{
    [Fact]
    public async Task TestUserNotFound()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var createContactForDimaWithNobodyCommand = new CreateContactCommand(dima.Response.Id, Guid.NewGuid().ToString());
        var createContactForDimaWithNobodyResult = await CreateContactCommandHandler.HandleAsync(createContactForDimaWithNobodyCommand);

        createContactForDimaWithNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
    
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var alice = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());

        var createContactForNobodyCommand = new CreateContactCommand(Guid.NewGuid().ToString(), alice.Response.Id);
        var createContactForNobodyResult = await CreateContactCommandHandler.HandleAsync(createContactForNobodyCommand);

        createContactForNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
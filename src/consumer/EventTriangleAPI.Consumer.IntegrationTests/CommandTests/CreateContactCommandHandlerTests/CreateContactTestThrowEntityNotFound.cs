using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandTests.CreateContactCommandHandlerTests;

public class CreateContactTestThrowEntityNotFound : IntegrationTestBase, IIntegrationTest
{
    [Fact]
    public async Task Test()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CommandHelper.CreateUserAliceCommand());

        var createContactForNobodyCommand = new CreateContactCommand(Guid.NewGuid().ToString(), alice.Response.Id);
        var createContactForDimaWithNobodyCommand = new CreateContactCommand(dima.Response.Id, Guid.NewGuid().ToString());
        
        var createContactForNobodyResult = await CreateContactCommandHandler.HandleAsync(createContactForNobodyCommand);
        var createContactForDimaWithNobodyResult = await CreateContactCommandHandler.HandleAsync(createContactForDimaWithNobodyCommand);

        createContactForNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
        createContactForDimaWithNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}
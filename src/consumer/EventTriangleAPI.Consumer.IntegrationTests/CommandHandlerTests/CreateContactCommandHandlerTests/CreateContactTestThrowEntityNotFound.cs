using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.CreateContactCommandHandlerTests;

public class CreateContactTestThrowEntityNotFound(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestUserNotFound()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var createContactForDimaWithNobodyCommand = new CreateContactCommand(dima.Response.Id, Guid.NewGuid().ToString());
        var createContactForDimaWithNobodyResult = await Fixture.CreateContactCommandHandler.HandleAsync(createContactForDimaWithNobodyCommand);

        createContactForDimaWithNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }

    [Fact]
    public async Task TestRequesterNotFound()
    {
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());

        var createContactForNobodyCommand = new CreateContactCommand(Guid.NewGuid().ToString(), alice.Response.Id);
        var createContactForNobodyResult = await Fixture.CreateContactCommandHandler.HandleAsync(createContactForNobodyCommand);

        createContactForNobodyResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}

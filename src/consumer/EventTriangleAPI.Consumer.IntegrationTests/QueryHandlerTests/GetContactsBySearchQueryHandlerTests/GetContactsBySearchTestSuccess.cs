using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetContactsBySearchQueryHandlerTests;

public class GetContactsBySearchTestSuccess(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var bob = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());
        var createContactForDimaWithAliceCommand = new CreateContactCommand(dima.Response.Id, alice.Response.Id);
        await Fixture.CreateContactCommandHandler.HandleAsync(createContactForDimaWithAliceCommand);

        var getContactsForDimaSearchAliceQuery = new GetContactsBySearchQuery(dima.Response.Id, alice.Response.Email);
        var getContactsForDimaSearchAliceResult = await Fixture.GetContactsBySearchQueryHandler.HandleAsync(getContactsForDimaSearchAliceQuery);
        var getContactsForDimaSearchBobQuery = new GetContactsBySearchQuery(dima.Response.Id, bob.Response.Email);
        var getContactsForDimaSearchBobResult = await Fixture.GetContactsBySearchQueryHandler.HandleAsync(getContactsForDimaSearchBobQuery);

        getContactsForDimaSearchAliceResult.Response.Count.Should().Be(0);
        getContactsForDimaSearchBobResult.Response.Count.Should().Be(1);
        getContactsForDimaSearchBobResult.Response.FirstOrDefault(x => x.ContactId == bob.Response.Id).Should().NotBeNull();
    }
}

using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetContactsQueryHandlerTests;

public class GetContactsTestSuccess(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var bob = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());
        var createContactForDimaWithAliceCommand = new CreateContactCommand(dima.Response.Id, alice.Response.Id);
        var createContactForDimaWithBobCommand = new CreateContactCommand(dima.Response.Id, bob.Response.Id);
        var createContactForAliceWithDimaCommand = new CreateContactCommand(alice.Response.Id, dima.Response.Id);
        await Fixture.CreateContactCommandHandler.HandleAsync(createContactForDimaWithAliceCommand);
        await Fixture.CreateContactCommandHandler.HandleAsync(createContactForDimaWithBobCommand);
        await Fixture.CreateContactCommandHandler.HandleAsync(createContactForAliceWithDimaCommand);

        var getContactsForDimaQuery = new GetContactsQuery(dima.Response.Id);
        var getContactsForAliceQuery = new GetContactsQuery(alice.Response.Id);
        var getContactsForBobQuery = new GetContactsQuery(bob.Response.Id);
        var getContactsForDimaResult = await Fixture.GetContactsQueryHandler.HandleAsync(getContactsForDimaQuery);
        var getContactsForAliceResult = await Fixture.GetContactsQueryHandler.HandleAsync(getContactsForAliceQuery);
        var getContactsForBobResult = await Fixture.GetContactsQueryHandler.HandleAsync(getContactsForBobQuery);

        getContactsForDimaResult.Response.Count.Should().Be(2);
        getContactsForDimaResult.Response.ForEach(x => x.UserId.Should().Be(dima.Response.Id));
        getContactsForDimaResult.Response.FirstOrDefault(x => x.ContactId == alice.Response.Id).Should().NotBeNull();
        getContactsForDimaResult.Response.FirstOrDefault(x => x.ContactId == bob.Response.Id).Should().NotBeNull();
        getContactsForAliceResult.Response.Count.Should().Be(1);
        getContactsForAliceResult.Response.ForEach(x => x.UserId.Should().Be(alice.Response.Id));
        getContactsForAliceResult.Response.FirstOrDefault(x => x.ContactId == dima.Response.Id).Should().NotBeNull();
        getContactsForBobResult.Response.Count.Should().Be(0);
    }
}

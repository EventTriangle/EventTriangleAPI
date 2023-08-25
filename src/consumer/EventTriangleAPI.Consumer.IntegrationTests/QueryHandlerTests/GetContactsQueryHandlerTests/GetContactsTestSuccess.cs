using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetContactsQueryHandlerTests;

public class GetContactsTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());
        var alice = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var bob = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());
        var createContactForDimaWithAliceCommand = new CreateContactCommand(dima.Response.Id, alice.Response.Id);
        var createContactForDimaWithBobCommand = new CreateContactCommand(dima.Response.Id, bob.Response.Id);
        var createContactForAliceWithDimaCommand = new CreateContactCommand(alice.Response.Id, dima.Response.Id);
        await CreateContactCommandHandler.HandleAsync(createContactForDimaWithAliceCommand);
        await CreateContactCommandHandler.HandleAsync(createContactForDimaWithBobCommand);
        await CreateContactCommandHandler.HandleAsync(createContactForAliceWithDimaCommand);

        var getContactsForDimaQuery = new GetContactsQuery(dima.Response.Id);
        var getContactsForAliceQuery = new GetContactsQuery(alice.Response.Id);
        var getContactsForBobQuery = new GetContactsQuery(bob.Response.Id);
        var getContactsForDimaResult = await GetContactsQueryHandler.HandleAsync(getContactsForDimaQuery);
        var getContactsForAliceResult = await GetContactsQueryHandler.HandleAsync(getContactsForAliceQuery);
        var getContactsForBobResult = await GetContactsQueryHandler.HandleAsync(getContactsForBobQuery);

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
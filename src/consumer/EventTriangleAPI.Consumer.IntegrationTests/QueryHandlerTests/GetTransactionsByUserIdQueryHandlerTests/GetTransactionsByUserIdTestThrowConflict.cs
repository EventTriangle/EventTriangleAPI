using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetTransactionsByUserIdQueryHandlerTests;

public class GetTransactionsByUserIdTestThrowConflict(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestRequesterIsNotAdmin()
    {
        var alice = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());
        var bob = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserBobCommand());

        var getTransactionsQuery = new GetTransactionsByUserIdQuery(alice.Response.Id, bob.Response.Id, 10, DateTime.UtcNow);
        var getTransactionsResult = await Fixture.GetTransactionsByUserIdQueryHandler.HandleAsync(getTransactionsQuery);

        getTransactionsResult.Error.Should().BeOfType<ConflictError>();
    }
}

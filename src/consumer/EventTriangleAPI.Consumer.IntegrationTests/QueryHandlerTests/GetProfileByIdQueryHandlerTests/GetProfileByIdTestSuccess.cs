using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetProfileByIdQueryHandlerTests;

public class GetProfileByIdTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var alice = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserAliceCommand());

        var getProfileByIdQuery = new GetProfileByIdQuery(alice.Response.Id);
        var getProfileByIdResult = await GetProfileByIdQueryHandler.HandleAsync(getProfileByIdQuery);

        getProfileByIdResult.Response.Id.Should().Be(alice.Response.Id);
        getProfileByIdResult.Response.Email.Should().Be(alice.Response.Email);
        getProfileByIdResult.Response.UserRole.Should().Be(alice.Response.UserRole);
        getProfileByIdResult.Response.UserStatus.Should().Be(alice.Response.UserStatus);
    }
}
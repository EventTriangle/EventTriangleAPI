using EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;
using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.QueryHandlerTests.GetProfileQueryHandlerTests;

public class GetProfileTestSuccess : IntegrationTestBase
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var getProfileQuery = new GetProfileQuery(dima.Response.Id);
        var getProfileResult = await GetProfileQueryHandler.HandleAsync(getProfileQuery);

        getProfileResult.Response.Id.Should().Be(dima.Response.Id);
        getProfileResult.Response.Email.Should().Be(dima.Response.Email);
        getProfileResult.Response.UserRole.Should().Be(dima.Response.UserRole);
        getProfileResult.Response.UserStatus.Should().Be(dima.Response.UserStatus);
    }
}
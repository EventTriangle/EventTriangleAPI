using FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;
using FluxPayAPI.Consumer.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests.QueryHandlerTests.GetProfileQueryHandlerTests;

public class GetProfileTestSuccess(TestFixture fixture) : TestBase(fixture)
{
    [Fact]
    public async Task TestSuccess()
    {
        var dima = await Fixture.CreateUserCommandHandler.HandleAsync(CreateUserCommandHelper.CreateUserDimaCommand());

        var getProfileQuery = new GetProfileQuery(dima.Response.Id);
        var getProfileResult = await Fixture.GetProfileQueryHandler.HandleAsync(getProfileQuery);

        getProfileResult.Response.Id.Should().Be(dima.Response.Id);
        getProfileResult.Response.Email.Should().Be(dima.Response.Email);
        getProfileResult.Response.UserRole.Should().Be(dima.Response.UserRole);
        getProfileResult.Response.UserStatus.Should().Be(dima.Response.UserStatus);
    }
}

using EventTriangleAPI.Shared.Application.Services;
using FluentAssertions;
using Xunit;


namespace EventTriangleAPI.Authorization.UnitTests.Services;

public class AppSettingsServiceTest
{
    [Fact]
    public void Test()
    {
        var appSettingsService = new AppSettingsService();

        var path = appSettingsService.GetAppSettingsPathAuthorization();

        File.Exists(path).Should().BeTrue();
    }
}

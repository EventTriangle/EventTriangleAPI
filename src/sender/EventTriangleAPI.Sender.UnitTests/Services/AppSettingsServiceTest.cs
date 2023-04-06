using EventTriangleAPI.Shared.Application.Services;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Sender.UnitTests.Services;

public class AppSettingsServiceTest
{
    [Fact]
    public void Test()
    {
        var appSettingsService = new AppSettingsService();

        var path = appSettingsService.GetAppSettingsPathSender();

        File.Exists(path).Should().BeTrue();
    }
}
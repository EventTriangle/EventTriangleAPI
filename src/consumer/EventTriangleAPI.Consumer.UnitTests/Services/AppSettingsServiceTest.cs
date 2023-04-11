using EventTriangleAPI.Shared.Application.Services;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.UnitTests.Services;

public class AppSettingsServiceTest
{
    [Fact]
    public void Test()
    {
        var appSettingsService = new AppSettingsService();

        var path = appSettingsService.GetAppSettingsPathConsumer();
        
        File.Exists(path).Should().BeTrue();
    }
}
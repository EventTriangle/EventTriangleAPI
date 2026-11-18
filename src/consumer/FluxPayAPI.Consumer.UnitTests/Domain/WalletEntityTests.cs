using FluxPayAPI.Consumer.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace FluxPayAPI.Consumer.UnitTests.Domain;

public class WalletEntityTests
{
    [Fact]
    public void TestSuccess()
    {
        var result = WalletEntityHelper.CreateSuccess;

        result.Should().NotThrow();
    }
}
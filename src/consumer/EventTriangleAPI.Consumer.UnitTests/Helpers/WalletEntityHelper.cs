using EventTriangleAPI.Consumer.Domain.Entities;

namespace EventTriangleAPI.Consumer.UnitTests.Helpers;

public class WalletEntityHelper
{
    public static WalletEntity CreateSuccess()
    {
        return new WalletEntity(0);
    }
}
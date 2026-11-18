using FluxPayAPI.Consumer.Domain.Entities;

namespace FluxPayAPI.Consumer.UnitTests.Helpers;

public class WalletEntityHelper
{
    public static WalletEntity CreateSuccess()
    {
        return new WalletEntity(0);
    }
}
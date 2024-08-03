using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests;

[Collection("Test collection")]
public class TestBase(TestFixture fixture) : IAsyncLifetime
{
    internal readonly TestFixture Fixture = fixture;

    public async Task InitializeAsync()
    {
        await Fixture.DatabaseContextFixture.Database.MigrateAsync();

        const string sql = "TRUNCATE TABLE \"ContactEntities\" CASCADE;" +
                           "TRUNCATE TABLE \"CreditCardEntities\" CASCADE;" +
                           "TRUNCATE TABLE \"SupportTicketEntities\" CASCADE;" +
                           "TRUNCATE TABLE \"TransactionEntities\" CASCADE;" +
                           "TRUNCATE TABLE \"UserEntities\" CASCADE;" +
                           "TRUNCATE TABLE \"WalletEntities\" CASCADE;";

        await Fixture.DatabaseContextFixture.Database.ExecuteSqlRawAsync(sql);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}

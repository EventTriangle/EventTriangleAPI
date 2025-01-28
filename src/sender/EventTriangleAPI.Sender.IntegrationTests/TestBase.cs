using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventTriangleAPI.Sender.IntegrationTests;

[Collection("Test collection")]
public class TestBase(TestFixture fixture) : IAsyncLifetime
{
    internal readonly TestFixture Fixture = fixture;

    public async Task InitializeAsync()
    {
        await Fixture.DatabaseContextFixture.Database.MigrateAsync();

        const string sql = "TRUNCATE TABLE \"ContactCreatedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"ContactDeletedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"CreditCardAddedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"CreditCardChangedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"CreditCardDeletedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"SupportTicketOpenedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"SupportTicketResolvedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"TransactionCardToUserCreatedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"TransactionUserToUserCreatedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"TransactionRollBackedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"UserCreatedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"UserSuspendedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"UserNotSuspendedEvents\" CASCADE;" +
                           "TRUNCATE TABLE \"UserRoleUpdatedEvents\" CASCADE;";

        await Fixture.DatabaseContextFixture.Database.ExecuteSqlRawAsync(sql);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}

using FluentAssertions;
using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;
using FluxPayAPI.Consumer.Domain.Entities;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Shared.DTO.Enums;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests;

public class PaginationCursorTests
{
    [Fact]
    public async Task TransactionAndTicketPagesIncludeTiesAndSubMillisecondRecords()
    {
        var connection = Environment.GetEnvironmentVariable("FLUXPAY_PAGINATION_TEST_DATABASE");
        PostgreSqlContainer container = null;
        if (string.IsNullOrWhiteSpace(connection))
        {
            container = new PostgreSqlBuilder("postgres:16").Build();
            await container.StartAsync();
            connection = container.GetConnectionString();
        }
        try
        {
            await using var db = new DatabaseContext(new DbContextOptionsBuilder<DatabaseContext>().UseNpgsql(connection).Options);
            await db.Database.MigrateAsync();
            var adminWallet = new WalletEntity(0);
            var userWallet = new WalletEntity(0);
            var admin = new UserEntity("admin", "admin@example.com", adminWallet.Id, UserRole.Admin, UserStatus.Active);
            var user = new UserEntity("user", "user@example.com", userWallet.Id, UserRole.User, UserStatus.Active);
            db.AddRange(adminWallet, userWallet, admin, user);
            var timestamp = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddTicks(1230);
            var transactions = Enumerable.Range(0, 7).Select(i => new TransactionEntity(user.Id, user.Id, 1,
                TransactionType.FromCardToUser, timestamp.AddTicks(i < 5 ? 0 : 10 * (i - 4)))).ToArray();
            var tickets = transactions.Select(t => new SupportTicketEntity(user.Id, userWallet.Id, t.Id, "Test", t.CreatedAt)).ToArray();
            db.AddRange(transactions);
            db.AddRange(tickets);
            await db.SaveChangesAsync();
            var initial = timestamp.AddMinutes(1);

            var history = new GetTransactionsQueryHandler(db);
            var bySearch = new GetTransactionsBySearchQueryHandler(db);
            var byUser = new GetTransactionsByUserIdQueryHandler(db);
            var support = new GetSupportTicketsQueryHandler(db);
            var allTickets = new GetTicketsQueryHandler(db);

            var historyIds = await ReadTransactions((date, id) => history.HandleAsync(new GetTransactionsQuery(user.Id, 2, date, id)), initial);
            var searchIds = await ReadTransactions((date, id) => bySearch.HandleAsync(new GetTransactionsBySearchQuery(user.Id, "-", 2, date, id)), initial);
            var userIds = await ReadTransactions((date, id) => byUser.HandleAsync(new GetTransactionsByUserIdQuery(admin.Id, user.Id, 2, date, id)), initial);
            var supportIds = await ReadTickets((date, id) => support.HandleAsync(new GetSupportsTicketsQuery(user.Id, 2, date, id)), initial);
            var adminTicketIds = await ReadTickets((date, id) => allTickets.HandleAsync(new GetTicketsQuery(admin.Id, 2, date, id)), initial);

            historyIds.Should().BeEquivalentTo(transactions.Select(x => x.Id));
            searchIds.Should().BeEquivalentTo(transactions.Select(x => x.Id));
            userIds.Should().BeEquivalentTo(transactions.Select(x => x.Id));
            supportIds.Should().BeEquivalentTo(tickets.Select(x => x.Id));
            adminTicketIds.Should().BeEquivalentTo(tickets.Select(x => x.Id));
        }
        finally { if (container != null) await container.DisposeAsync(); }
    }

    private static async Task<List<Guid>> ReadTransactions(
        Func<DateTime, Guid?, Task<FluxPayAPI.Shared.DTO.Abstractions.IResult<List<TransactionDto>, FluxPayAPI.Shared.DTO.Responses.Errors.Error>>> fetch, DateTime date)
    {
        var ids = new List<Guid>();
        Guid? cursor = null;
        for (var i = 0; i < 10; i++)
        {
            var page = (await fetch(date, cursor)).Response;
            if (page.Count == 0) break;
            ids.AddRange(page.Select(x => x.Id));
            date = page[^1].CreatedAt;
            cursor = page[^1].Id;
        }
        return ids;
    }

    private static async Task<List<Guid>> ReadTickets(
        Func<DateTime, Guid?, Task<FluxPayAPI.Shared.DTO.Abstractions.IResult<List<SupportTicketDto>, FluxPayAPI.Shared.DTO.Responses.Errors.Error>>> fetch, DateTime date)
    {
        var ids = new List<Guid>();
        Guid? cursor = null;
        for (var i = 0; i < 10; i++)
        {
            var page = (await fetch(date, cursor)).Response;
            if (page.Count == 0) break;
            ids.AddRange(page.Select(x => x.Id));
            date = page[^1].CreatedAt;
            cursor = page[^1].Id;
        }
        return ids;
    }
}

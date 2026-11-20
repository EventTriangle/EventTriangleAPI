using System.Net;
using System.Security.Claims;
using FluxPayAPI.Authorization.BusinessLogic.Services;
using FluxPayAPI.Authorization.Domain.Entities;
using FluxPayAPI.Authorization.Persistence;
using FluxPayAPI.Shared.DTO.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace FluxPayAPI.Authorization.IntegrationTests;

public class TicketStoreTests
{
    [Fact]
    public async Task ExpiredCachedTicketWithoutRefreshTokenEndsSession()
    {
        using var fixture = new Fixture();
        var id = Guid.NewGuid();
        var expired = Ticket(DateTimeOffset.UtcNow.AddMinutes(-1));
        await fixture.Save(id, expired);
        fixture.Cache.Set(id.ToString(), expired);

        Assert.Null(await fixture.Store.RetrieveAsync(id.ToString()));
        Assert.False(fixture.Cache.TryGetValue(id.ToString(), out _));
        using var scope = fixture.Services.CreateScope();
        Assert.Null(await scope.ServiceProvider.GetRequiredService<DatabaseContext>().UserSessions.FindAsync(id));
    }

    [Fact]
    public async Task RemovingSessionEvictsCachedTicket()
    {
        using var fixture = new Fixture();
        var id = Guid.NewGuid();
        var ticket = Ticket(DateTimeOffset.UtcNow.AddMinutes(5));
        await fixture.Save(id, ticket);
        fixture.Cache.Set(id.ToString(), ticket);

        await fixture.Store.RemoveAsync(id.ToString());

        Assert.Null(await fixture.Store.RetrieveAsync(id.ToString()));
    }

    [Fact]
    public async Task ExpiredCachedTicketRefreshesAndReturnsNewAccessToken()
    {
        using var fixture = new Fixture();
        var id = Guid.NewGuid();
        var expired = Ticket(DateTimeOffset.UtcNow.AddMinutes(-1));
        expired.Properties.StoreTokens(new[] {
            new AuthenticationToken { Name = "refresh_token", Value = "old-refresh" },
            new AuthenticationToken { Name = "access_token", Value = "old-access" },
            new AuthenticationToken { Name = "id_token", Value = "old-id" }
        });
        await fixture.Save(id, expired);
        fixture.Cache.Set(id.ToString(), expired);

        var renewed = await fixture.Store.RetrieveAsync(id.ToString());

        Assert.NotNull(renewed);
        Assert.Equal("new-access", renewed.Properties.GetTokenValue("access_token"));
        Assert.True(renewed.Properties.ExpiresUtc > DateTimeOffset.UtcNow);
    }

    private static AuthenticationTicket Ticket(DateTimeOffset expires) => new(
        new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("sub", "user") }, "test")),
        new AuthenticationProperties { ExpiresUtc = expires }, "Cookies");

    private sealed class Fixture : IDisposable
    {
        public ServiceProvider Services { get; }
        public MemoryCache Cache { get; } = new(new MemoryCacheOptions());
        public TicketStore Store { get; }
        private readonly TicketSerializer serializer = new();

        public Fixture()
        {
            var databaseName = Guid.NewGuid().ToString();
            Services = new ServiceCollection()
                .AddDbContext<DatabaseContext>(o => o.UseInMemoryDatabase(databaseName))
                .BuildServiceProvider();
            Store = new TicketStore("http://localhost:12345", Services.GetRequiredService<IServiceScopeFactory>(),
                serializer, new HttpClient(new TokenHandler()), new AzureAdConfiguration {
                    AzureAdTokenUrl = "https://example.test/token", ClientSecret = "test", Scopes = "test"
                }, Cache, NullLoggerFactory.Instance);
        }

        public async Task Save(Guid id, AuthenticationTicket ticket)
        {
            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            db.Users.Add(new UserEntity("user", "test@example.com"));
            db.UserSessions.Add(new UserSessionEntity(id, ticket.Properties.ExpiresUtc!.Value, serializer.Serialize(ticket), "user"));
            await db.SaveChangesAsync();
        }

        public void Dispose() { Services.Dispose(); Cache.Dispose(); }
    }

    private sealed class TokenHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StringContent("{\"access_token\":\"new-access\",\"refresh_token\":\"new-refresh\",\"id_token\":\"new-id\",\"expires_in\":3600,\"token_type\":\"Bearer\"}", System.Text.Encoding.UTF8, "application/json")
            });
    }
}

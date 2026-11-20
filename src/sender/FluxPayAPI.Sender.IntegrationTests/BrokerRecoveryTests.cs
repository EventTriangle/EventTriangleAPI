using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using FluentAssertions;
using FluxPayAPI.Sender.BusinessLogic.CommandHandlers;
using FluxPayAPI.Sender.Persistence;
using FluxPayAPI.Shared.DTO.Enums;
using FluxPayAPI.Shared.DTO.Messages;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Xunit;

namespace FluxPayAPI.Sender.IntegrationTests;

public class BrokerRecoveryTests
{
    [Fact]
    public async Task QueuedEventIsDeliveredAfterBrokerConnectionReturns()
    {
        var dbConnection = Environment.GetEnvironmentVariable("FLUXPAY_OUTBOX_TEST_DATABASE");
        var rabbitHost = Environment.GetEnvironmentVariable("FLUXPAY_TEST_RABBIT_HOST");
        var rabbitPort = Environment.GetEnvironmentVariable("FLUXPAY_TEST_RABBIT_PORT");
        var rabbitUser = Environment.GetEnvironmentVariable("FLUXPAY_TEST_RABBIT_USER");
        var rabbitPassword = Environment.GetEnvironmentVariable("FLUXPAY_TEST_RABBIT_PASSWORD");
        var rabbitVhost = Environment.GetEnvironmentVariable("FLUXPAY_TEST_RABBIT_VHOST") ?? "/";
        PostgreSqlContainer postgres = null;
        RabbitMqContainer rabbit = null;
        try
        {
            if (string.IsNullOrWhiteSpace(dbConnection))
            {
                postgres = new PostgreSqlBuilder("postgres:16").Build();
                await postgres.StartAsync();
                dbConnection = postgres.GetConnectionString();
            }
            if (string.IsNullOrWhiteSpace(rabbitHost))
            {
                rabbitUser = "probe";
                rabbitPassword = "probe-password";
                rabbit = new RabbitMqBuilder("rabbitmq:4-management").WithUsername(rabbitUser).WithPassword(rabbitPassword).Build();
                await rabbit.StartAsync();
                rabbitHost = rabbit.Hostname;
                rabbitPort = rabbit.GetMappedPublicPort(5672).ToString();
            }

            await using var proxy = new BrokerProxy(rabbitHost, int.Parse(rabbitPort!));
            var delivered = new TaskCompletionSource<UserCreatedEventMessage>(TaskCreationOptions.RunContinuationsAsynchronously);
            using var host = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddSingleton(delivered);
                services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(dbConnection));
                services.AddScoped<CreateUserCommandHandler>();
                services.Configure<MassTransitHostOptions>(options => { options.WaitUntilStarted = true; options.StartTimeout = TimeSpan.FromSeconds(30); });
                services.AddMassTransit(config =>
                {
                    config.AddConsumer<ProbeConsumer>();
                    config.AddEntityFrameworkOutbox<DatabaseContext>(outbox =>
                    {
                        outbox.UsePostgres();
                        outbox.UseBusOutbox();
                        outbox.QueryDelay = TimeSpan.FromMilliseconds(100);
                    });
                    config.UsingRabbitMq((context, bus) =>
                    {
                        bus.Host("127.0.0.1", (ushort)proxy.Port, rabbitVhost, h =>
                        {
                            h.Username(rabbitUser!);
                            h.Password(rabbitPassword!);
                        });
                        bus.ConfigureEndpoints(context);
                    });
                });
            }).Build();

            await using (var scope = host.Services.CreateAsyncScope())
                await scope.ServiceProvider.GetRequiredService<DatabaseContext>().Database.MigrateAsync();
            await host.StartAsync();
            proxy.Disconnect();
            var userId = Guid.NewGuid().ToString();
            await using (var scope = host.Services.CreateAsyncScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var result = await scope.ServiceProvider.GetRequiredService<CreateUserCommandHandler>().HandleAsync(
                    new CreateUserCommand(userId, "test@example.com", UserRole.User, UserStatus.Active));
                result.IsSuccess.Should().BeTrue();
                (await db.UserCreatedEvents.CountAsync(x => x.UserId == userId)).Should().Be(1);
                (await db.Set<OutboxMessage>().CountAsync()).Should().Be(1);
                delivered.Task.IsCompleted.Should().BeFalse();
            }
            proxy.Reconnect();
            (await delivered.Task.WaitAsync(TimeSpan.FromSeconds(60))).UserId.Should().Be(userId);
            await host.StopAsync();
        }
        finally
        {
            if (rabbit != null) await rabbit.DisposeAsync();
            if (postgres != null) await postgres.DisposeAsync();
        }
    }

    public class ProbeConsumer(TaskCompletionSource<UserCreatedEventMessage> delivered) : IConsumer<UserCreatedEventMessage>
    {
        public Task Consume(ConsumeContext<UserCreatedEventMessage> context)
        {
            delivered.TrySetResult(context.Message);
            return Task.CompletedTask;
        }
    }

    private sealed class BrokerProxy : IAsyncDisposable
    {
        private readonly TcpListener listener = new(IPAddress.Loopback, 0);
        private readonly CancellationTokenSource stop = new();
        private readonly ConcurrentDictionary<TcpClient, byte> connections = new();
        private readonly string targetHost;
        private readonly int targetPort;
        private volatile bool online = true;
        private readonly Task acceptLoop;
        public int Port => ((IPEndPoint)listener.LocalEndpoint).Port;

        public BrokerProxy(string targetHost, int targetPort)
        {
            this.targetHost = targetHost;
            this.targetPort = targetPort;
            listener.Start();
            acceptLoop = AcceptAsync();
        }

        public void Disconnect()
        {
            online = false;
            foreach (var connection in connections.Keys) connection.Dispose();
        }

        public void Reconnect() => online = true;

        private async Task AcceptAsync()
        {
            try
            {
                while (!stop.IsCancellationRequested)
                {
                    var client = await listener.AcceptTcpClientAsync(stop.Token);
                    if (!online) { client.Dispose(); continue; }
                    _ = ForwardAsync(client);
                }
            }
            catch (OperationCanceledException) { }
            catch (ObjectDisposedException) { }
        }

        private async Task ForwardAsync(TcpClient client)
        {
            using (client)
            using (var upstream = new TcpClient())
            {
                connections.TryAdd(client, 0);
                connections.TryAdd(upstream, 0);
                try
                {
                    if (!online) return;
                    await upstream.ConnectAsync(targetHost, targetPort, stop.Token);
                    var a = client.GetStream().CopyToAsync(upstream.GetStream(), stop.Token);
                    var b = upstream.GetStream().CopyToAsync(client.GetStream(), stop.Token);
                    await Task.WhenAny(a, b);
                }
                catch (Exception e) when (e is IOException or SocketException or ObjectDisposedException or OperationCanceledException) { }
                finally
                {
                    connections.TryRemove(client, out _);
                    connections.TryRemove(upstream, out _);
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            Disconnect();
            stop.Cancel();
            listener.Stop();
            await acceptLoop;
            stop.Dispose();
        }
    }
}

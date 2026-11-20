using FluentAssertions;
using FluxPayAPI.Consumer.Domain.Entities;
using FluxPayAPI.Consumer.Persistence;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests;

public class InboxDeduplicationTests
{
    public record ProbeMessage(Guid OperationId);

    public class ProbeState
    {
        public int Count;
        public TaskCompletionSource<bool> FirstDelivery { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    public class ProbeConsumer(DatabaseContext db, ProbeState state) : IConsumer<ProbeMessage>
    {
        public async Task Consume(ConsumeContext<ProbeMessage> context)
        {
            Interlocked.Increment(ref state.Count);
            db.WalletEntities.Add(new WalletEntity(0));
            await db.SaveChangesAsync();
            state.FirstDelivery.TrySetResult(true);
        }
    }

    [Fact]
    public async Task DuplicateMessageIsNotAppliedTwice()
    {
        var connection = Environment.GetEnvironmentVariable("FLUXPAY_INBOX_TEST_DATABASE");
        PostgreSqlContainer container = null;
        if (string.IsNullOrWhiteSpace(connection))
        {
            container = new PostgreSqlBuilder("postgres:16").Build();
            await container.StartAsync();
            connection = container.GetConnectionString();
        }

        try
        {
            var state = new ProbeState();
            using var host = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddSingleton(state);
                services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connection));
                services.AddMassTransit(config =>
                {
                    config.AddConsumer<ProbeConsumer>();
                    config.AddEntityFrameworkOutbox<DatabaseContext>(outbox => outbox.UsePostgres());
                    config.UsingInMemory((context, bus) => bus.ReceiveEndpoint("inbox-probe", endpoint =>
                    {
                        endpoint.UseEntityFrameworkOutbox<DatabaseContext>(context);
                        endpoint.ConfigureConsumer<ProbeConsumer>(context);
                    }));
                });
            }).Build();

            await using (var scope = host.Services.CreateAsyncScope())
                await scope.ServiceProvider.GetRequiredService<DatabaseContext>().Database.MigrateAsync();
            await host.StartAsync();
            var endpoint = await host.Services.GetRequiredService<IBus>().GetSendEndpoint(new Uri("queue:inbox-probe"));
            var messageId = Guid.NewGuid();
            var message = new ProbeMessage(Guid.NewGuid());
            await endpoint.Send(message, context => context.MessageId = messageId);
            await state.FirstDelivery.Task.WaitAsync(TimeSpan.FromSeconds(20));
            await endpoint.Send(message, context => context.MessageId = messageId);

            var deadline = DateTime.UtcNow.AddSeconds(20);
            var receivedTwice = false;
            while (DateTime.UtcNow < deadline && !receivedTwice)
            {
                await using var scope = host.Services.CreateAsyncScope();
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                receivedTwice = await db.Set<InboxState>().AnyAsync(x => x.MessageId == messageId && x.ReceiveCount >= 2);
                if (!receivedTwice) await Task.Delay(100);
            }

            receivedTwice.Should().BeTrue("the duplicate must reach the inbox before its effect is checked");
            state.Count.Should().Be(1);
            await using (var scope = host.Services.CreateAsyncScope())
                (await scope.ServiceProvider.GetRequiredService<DatabaseContext>().WalletEntities.CountAsync()).Should().Be(1);
            await host.StopAsync();
        }
        finally
        {
            if (container != null) await container.DisposeAsync();
        }
    }
}

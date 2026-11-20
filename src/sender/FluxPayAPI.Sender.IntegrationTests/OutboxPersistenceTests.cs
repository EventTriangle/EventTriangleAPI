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
using Xunit;

namespace FluxPayAPI.Sender.IntegrationTests;

public class OutboxPersistenceTests
{
    [Fact]
    public async Task PersistsEventBeforeTransportStartsAndDeliversItAfterwards()
    {
        var externalConnection = Environment.GetEnvironmentVariable("FLUXPAY_OUTBOX_TEST_DATABASE");
        PostgreSqlContainer container = null;
        if (string.IsNullOrWhiteSpace(externalConnection))
        {
            container = new PostgreSqlBuilder("postgres:16").Build();
            await container.StartAsync();
            externalConnection = container.GetConnectionString();
        }

        try
        {
            var delivered = new TaskCompletionSource<UserCreatedEventMessage>(TaskCreationOptions.RunContinuationsAsynchronously);
            using var host = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddSingleton(delivered);
                services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(externalConnection));
                services.AddScoped<CreateUserCommandHandler>();
                services.AddMassTransit(config =>
                {
                    config.AddConsumer<ProbeConsumer>();
                    config.AddEntityFrameworkOutbox<DatabaseContext>(outbox =>
                    {
                        outbox.UsePostgres();
                        outbox.UseBusOutbox();
                        outbox.QueryDelay = TimeSpan.FromMilliseconds(100);
                    });
                    config.UsingInMemory((context, bus) => bus.ConfigureEndpoints(context));
                });
            }).Build();

            var userId = Guid.NewGuid().ToString();
            await using (var scope = host.Services.CreateAsyncScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await db.Database.MigrateAsync();
                var handler = scope.ServiceProvider.GetRequiredService<CreateUserCommandHandler>();
                var result = await handler.HandleAsync(new CreateUserCommand(userId, "test@example.com", UserRole.User, UserStatus.Active));
                result.IsSuccess.Should().BeTrue();
                (await db.UserCreatedEvents.CountAsync(x => x.UserId == userId)).Should().Be(1);
                (await db.Set<OutboxMessage>().CountAsync()).Should().Be(1);
                delivered.Task.IsCompleted.Should().BeFalse();
            }

            await host.StartAsync();
            var message = await delivered.Task.WaitAsync(TimeSpan.FromSeconds(20));
            message.UserId.Should().Be(userId);
            await host.StopAsync();
        }
        finally
        {
            if (container != null) await container.DisposeAsync();
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
}

using FluxPayAPI.Consumer.BusinessLogic.Consumers;
using FluxPayAPI.Consumer.BusinessLogic.Hubs;
using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.Domain.Entities;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Consumer.Presentation.DependencyInjection;
using FluxPayAPI.Shared.DTO.Enums;
using FluxPayAPI.Shared.DTO.Messages;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Testcontainers.PostgreSql;
using Xunit;

namespace FluxPayAPI.Consumer.IntegrationTests;

public class SupportTicketNotificationTests
{
    [Fact]
    public async Task SendsCommittedTicketAndPaymentResultsToTheRightUsers()
    {
        await using var postgres = new PostgreSqlBuilder("postgres:16").Build();
        await postgres.StartAsync();
        var options = new DbContextOptionsBuilder<DatabaseContext>().UseNpgsql(postgres.GetConnectionString()).Options;
        await using var db = new DatabaseContext(options);
        await db.Database.MigrateAsync();

        var adminWallet = new WalletEntity(0);
        var userWallet = new WalletEntity(5);
        var admin = new UserEntity("admin", "admin@example.com", adminWallet.Id, UserRole.Admin, UserStatus.Active);
        var user = new UserEntity("user", "user@example.com", userWallet.Id, UserRole.User, UserStatus.Active);
        var transaction = new TransactionEntity(user.Id, user.Id, 1, TransactionType.FromCardToUser, DateTime.UtcNow);
        var ticket = new SupportTicketEntity(user.Id, userWallet.Id, transaction.Id, "Test", DateTime.UtcNow);
        db.AddRange(adminWallet, userWallet, admin, user, transaction, ticket);
        await db.SaveChangesAsync();

        var resolved = new TaskCompletionSource<SupportTicketDto>(TaskCreationOptions.RunContinuationsAsynchronously);
        var senderNotified = new TaskCompletionSource<(Guid? OperationId, decimal Balance, bool TransactionExists)>(TaskCreationOptions.RunContinuationsAsynchronously);
        var recipientNotified = new TaskCompletionSource<Guid?>(TaskCreationOptions.RunContinuationsAsynchronously);
        var requester = new Mock<INotificationHub>();
        requester.Setup(x => x.SupportTicketResolvedAsync(It.IsAny<SupportTicketDto>()))
            .Returns(async (SupportTicketDto dto) =>
            {
                await using var reader = new DatabaseContext(options);
                if (await reader.SupportTicketEntities.AnyAsync(t => t.Id == dto.Id && t.TicketStatus == TicketStatus.Resolved))
                    resolved.TrySetResult(dto);
            });
        requester.Setup(x => x.TransactionSucceededAsync(It.IsAny<TransactionDto>(), It.IsAny<Guid?>()))
            .Returns(async (TransactionDto dto, Guid? operationId) =>
            {
                await using var reader = new DatabaseContext(options);
                var wallet = await reader.WalletEntities.SingleAsync(w => w.Id == userWallet.Id);
                senderNotified.TrySetResult((operationId, wallet.Balance,
                    await reader.TransactionEntities.AnyAsync(t => t.Id == dto.Id)));
            });
        var recipient = new Mock<INotificationHub>();
        recipient.Setup(x => x.TransactionSucceededAsync(It.IsAny<TransactionDto>(), It.IsAny<Guid?>()))
            .Returns((TransactionDto _, Guid? operationId) =>
            {
                recipientNotified.TrySetResult(operationId);
                return Task.CompletedTask;
            });
        var clients = new Mock<IHubClients<INotificationHub>>();
        clients.Setup(x => x.User(user.Id)).Returns(requester.Object);
        clients.Setup(x => x.User(admin.Id)).Returns(recipient.Object);
        var hub = new Mock<IHubContext<NotificationHub, INotificationHub>>();
        hub.SetupGet(x => x.Clients).Returns(clients.Object);

        using var host = Host.CreateDefaultBuilder().ConfigureServices(services =>
        {
            services.AddSingleton(hub.Object);
            services.AddDbContext<DatabaseContext>(o => o.UseNpgsql(postgres.GetConnectionString()));
            services.AddCommandHandlers();
            services.AddMassTransit(config =>
            {
                config.AddConsumer<EventConsumer>();
                config.AddConsumer<NotificationConsumer>();
                config.AddEntityFrameworkOutbox<DatabaseContext>(outbox => outbox.UsePostgres());
                config.UsingInMemory((context, bus) =>
                {
                    bus.ReceiveEndpoint("notification-queue", endpoint => endpoint.ConfigureConsumer<NotificationConsumer>(context));
                    bus.ReceiveEndpoint("event-queue", endpoint =>
                    {
                        endpoint.UseEntityFrameworkOutbox<DatabaseContext>(context);
                        endpoint.ConfigureConsumer<EventConsumer>(context);
                    });
                });
            });
        }).Build();

        await host.StartAsync();
        try
        {
            var bus = host.Services.GetRequiredService<IBus>();
            await bus.Publish(new SupportTicketResolvedEventMessage(Guid.NewGuid(), admin.Id, ticket.Id, "Reviewed", DateTime.UtcNow));
            var result = await resolved.Task.WaitAsync(TimeSpan.FromSeconds(15));
            Assert.Equal(ticket.Id, result.Id);
            Assert.Equal("Reviewed", result.TicketJustification);
            recipient.Verify(x => x.SupportTicketResolvedAsync(It.IsAny<SupportTicketDto>()), Times.Never);

            var operationId = Guid.NewGuid();
            await bus.Publish(new TransactionUserToUserCreatedEventMessage(Guid.NewGuid(), user.Id, admin.Id, 2, DateTime.UtcNow, operationId));
            var payment = await senderNotified.Task.WaitAsync(TimeSpan.FromSeconds(15));
            Assert.Equal(operationId, payment.OperationId);
            Assert.Equal(3m, payment.Balance);
            Assert.True(payment.TransactionExists);
            Assert.Null(await recipientNotified.Task.WaitAsync(TimeSpan.FromSeconds(15)));
        }
        finally
        {
            await host.StopAsync();
        }
    }
}

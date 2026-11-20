using FluxPayAPI.Consumer.BusinessLogic.Hubs;
using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.BusinessLogic.Models.Notifications;
using FluxPayAPI.Consumer.Persistence;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FluxPayAPI.Consumer.BusinessLogic.Consumers;

public class NotificationConsumer(IHubContext<NotificationHub, INotificationHub> hub, DatabaseContext db) :
    IConsumer<CreditCardAddedNotice>,
    IConsumer<SupportTicketOpenedNotice>,
    IConsumer<SupportTicketResolvedNotice>,
    IConsumer<TransactionSucceededNotice>,
    IConsumer<TransactionCanceledNotice>
{
    public async Task Consume(ConsumeContext<CreditCardAddedNotice> context)
    {
        var message = context.Message;
        var card = await db.CreditCardEntities.AsNoTracking()
            .Where(card => card.Id == message.CardId && card.UserId == message.UserId)
            .Select(card => new CreditCardDto(card.Id, card.UserId, card.HolderName,
                card.CardNumber, card.Cvv, card.Expiration, card.PaymentNetwork))
            .SingleOrDefaultAsync(context.CancellationToken);

        if (card != null) await hub.Clients.User(message.UserId).CreditCardAddedAsync(card);
    }

    public Task Consume(ConsumeContext<SupportTicketOpenedNotice> context) =>
        hub.Clients.User(context.Message.UserId).SupportTicketOpenedAsync(context.Message.Ticket);

    public Task Consume(ConsumeContext<SupportTicketResolvedNotice> context) =>
        hub.Clients.User(context.Message.UserId).SupportTicketResolvedAsync(context.Message.Ticket);

    public Task Consume(ConsumeContext<TransactionSucceededNotice> context)
    {
        var message = context.Message;
        var requester = hub.Clients.User(message.RequesterId)
            .TransactionSucceededAsync(message.Transaction, message.OperationId);
        return message.ToUserId is { } recipient && recipient != message.RequesterId
            ? Task.WhenAll(requester, hub.Clients.User(recipient).TransactionSucceededAsync(message.Transaction, null))
            : requester;
    }

    public Task Consume(ConsumeContext<TransactionCanceledNotice> context) =>
        hub.Clients.User(context.Message.RequesterId).TransactionCanceledAsync(context.Message.Notification);
}

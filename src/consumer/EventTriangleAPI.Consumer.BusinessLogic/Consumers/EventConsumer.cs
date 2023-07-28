using EventTriangleAPI.Shared.DTO.Messages;
using MassTransit;

namespace EventTriangleAPI.Consumer.BusinessLogic.Consumers;

public class EventConsumer : 
    IConsumer<ContactCreatedEventMessage>,
    IConsumer<ContactDeletedEventMessage>,
    IConsumer<CreditCardAddedEventMessage>,
    IConsumer<CreditCardChangedEventMessage>,
    IConsumer<CreditCardDeletedEventMessage>,
    IConsumer<SupportTicketOpenedEventMessage>,
    IConsumer<SupportTicketResolvedEventMessage>,
    IConsumer<TransactionCreatedEventMessage>,
    IConsumer<TransactionRollBackedEventMessage>,
    IConsumer<UserCreatedEventMessage>,
    IConsumer<UserNotSuspendedEventMessage>,
    IConsumer<UserRoleUpdatedEventMessage>,
    IConsumer<UserSuspendedEventMessage>
{
    public Task Consume(ConsumeContext<ContactCreatedEventMessage> context)
    {
        throw new NotImplementedException();
    }

    public Task Consume(ConsumeContext<ContactDeletedEventMessage> context)
    {
        throw new NotImplementedException();
    }

    public Task Consume(ConsumeContext<CreditCardAddedEventMessage> context)
    {
        throw new NotImplementedException();
    }

    public Task Consume(ConsumeContext<CreditCardChangedEventMessage> context)
    {
        throw new NotImplementedException();
    }

    public Task Consume(ConsumeContext<CreditCardDeletedEventMessage> context)
    {
        throw new NotImplementedException();
    }

    public Task Consume(ConsumeContext<SupportTicketOpenedEventMessage> context)
    {
        throw new NotImplementedException();
    }

    public Task Consume(ConsumeContext<SupportTicketResolvedEventMessage> context)
    {
        throw new NotImplementedException();
    }

    public Task Consume(ConsumeContext<TransactionCreatedEventMessage> context)
    {
        throw new NotImplementedException();
    }

    public Task Consume(ConsumeContext<TransactionRollBackedEventMessage> context)
    {
        throw new NotImplementedException();
    }

    public Task Consume(ConsumeContext<UserCreatedEventMessage> context)
    {
        throw new NotImplementedException();
    }

    public Task Consume(ConsumeContext<UserNotSuspendedEventMessage> context)
    {
        throw new NotImplementedException();
    }

    public Task Consume(ConsumeContext<UserRoleUpdatedEventMessage> context)
    {
        throw new NotImplementedException();
    }

    public Task Consume(ConsumeContext<UserSuspendedEventMessage> context)
    {
        throw new NotImplementedException();
    }
}
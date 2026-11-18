using FluxPayAPI.Sender.Domain.Entities;
using FluxPayAPI.Sender.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public class TopUpAccountBalanceCommandHandler : ICommandHandler<TopUpAccountBalanceCommand, TransactionCardToUserCreatedEvent>
{
    private readonly DatabaseContext _context;
    private readonly IPublishEndpoint _client;
    
    public TopUpAccountBalanceCommandHandler(DatabaseContext context, IPublishEndpoint client)
    {
        _context = context;
        _client = client;
    }

    public async Task<IResult<TransactionCardToUserCreatedEvent, Error>> HandleAsync(TopUpAccountBalanceCommand command)
    {
        var transactionCreatedEvent = new TransactionCardToUserCreatedEvent(
            command.RequesterId,
            command.CreditCardId,
            command.Amount);

        _context.TransactionCardToUserCreatedEvents.Add(transactionCreatedEvent);
        await _context.SaveChangesAsync();
        
        var _ = _client.Publish(transactionCreatedEvent.CreateEventMessage());

        return new Result<TransactionCardToUserCreatedEvent>(transactionCreatedEvent);
    }
}
using FluxPayAPI.Sender.Domain.Entities;
using FluxPayAPI.Sender.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public class RollBackTransactionCommandHandler : ICommandHandler<RollBackTransactionCommand, TransactionRollBackedEvent>
{
    private readonly DatabaseContext _context;
    private readonly IPublishEndpoint _client;

    public RollBackTransactionCommandHandler(DatabaseContext context, IPublishEndpoint client)
    {
        _context = context;
        _client = client;
    }

    public async Task<IResult<TransactionRollBackedEvent, Error>> HandleAsync(RollBackTransactionCommand command)
    {
        var transactionRollBackedEvent = new TransactionRollBackedEvent(command.RequesterId, command.TransactionId);

        _context.TransactionRollBackedEvents.Add(transactionRollBackedEvent);
        await _context.SaveChangesAsync();
        
        var _ = _client.Publish(transactionRollBackedEvent.CreateEventMessage());

        return new Result<TransactionRollBackedEvent>(transactionRollBackedEvent);
    }
}
using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

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
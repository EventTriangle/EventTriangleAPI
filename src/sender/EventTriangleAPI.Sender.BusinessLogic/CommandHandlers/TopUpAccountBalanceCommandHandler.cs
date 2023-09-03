using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

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
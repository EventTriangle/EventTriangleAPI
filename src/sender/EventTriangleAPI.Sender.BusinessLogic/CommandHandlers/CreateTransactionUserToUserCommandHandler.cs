using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class CreateTransactionUserToUserCommandHandler : 
    ICommandHandler<CreateTransactionUserToUserCommand, TransactionUserToUserCreatedEvent>
{
    private readonly DatabaseContext _context;
    private readonly IPublishEndpoint _client;
    
    public CreateTransactionUserToUserCommandHandler(DatabaseContext context, IPublishEndpoint client)
    {
        _context = context;
        _client = client;
    }
    
    public async Task<IResult<TransactionUserToUserCreatedEvent, Error>> HandleAsync(CreateTransactionUserToUserCommand command)
    {
        var transactionCreatedEvent = new TransactionUserToUserCreatedEvent(
            command.RequesterId,
            command.ToUserId,
            command.Amount);

        _context.TransactionUserToUserCreatedEvents.Add(transactionCreatedEvent);
        await _context.SaveChangesAsync();
        
        var _ = _client.Publish(transactionCreatedEvent.CreateEventMessage());

        return new Result<TransactionUserToUserCreatedEvent>(transactionCreatedEvent);
    }
}
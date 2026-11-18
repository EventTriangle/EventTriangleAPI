using FluxPayAPI.Sender.Domain.Entities;
using FluxPayAPI.Sender.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

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
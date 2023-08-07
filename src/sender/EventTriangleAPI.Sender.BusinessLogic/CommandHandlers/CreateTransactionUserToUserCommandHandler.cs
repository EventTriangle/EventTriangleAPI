using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class CreateTransactionUserToUserCommandHandler : 
    ICommandHandler<CreateTransactionUserToUserCommand, TransactionUserToUserCreatedEvent>
{
    private readonly DatabaseContext _context;

    public CreateTransactionUserToUserCommandHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<IResult<TransactionUserToUserCreatedEvent, Error>> HandleAsync(CreateTransactionUserToUserCommand command)
    {
        var transactionCreatedEvent = new TransactionUserToUserCreatedEvent(
            command.FromUserId,
            command.ToUserId,
            command.Amount);

        _context.TransactionUserToUserCreatedEvents.Add(transactionCreatedEvent);
        await _context.SaveChangesAsync();
        
        new MockOrder().Send(transactionCreatedEvent);

        return new Result<TransactionUserToUserCreatedEvent>(transactionCreatedEvent);
    }
}
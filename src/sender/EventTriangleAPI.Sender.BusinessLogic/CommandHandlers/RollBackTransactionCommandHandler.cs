using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class RollBackTransactionCommandHandler : ICommandHandler<RollBackTransactionBody, string>
{
    private readonly DatabaseContext _context;

    public RollBackTransactionCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(ICommand<RollBackTransactionBody> command)
    {
        var transactionRollBackedEvent = new TransactionRollBackedEvent(command.Body.TransactionId);

        _context.TransactionRollBackedEvents.Add(transactionRollBackedEvent);
        await _context.SaveChangesAsync();
        
        new MockOrder().Send(transactionRollBackedEvent);

        return new Result<string>("");
    }
}
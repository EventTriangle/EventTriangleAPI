using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public class RollBackTransactionCommandHandler : ICommandHandler<RollBackTransactionCommand, TransactionEntity>
{
    private readonly DatabaseContext _context;

    public RollBackTransactionCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<TransactionEntity, Error>> HandleAsync(RollBackTransactionCommand command)
    {
        var transaction = await _context.TransactionEntities.FirstOrDefaultAsync(x => x.Id == command.TransactionId);

        if (transaction == null)
        {
            throw new NotImplementedException();
        }
        
        transaction.UpdateTransactionState(TransactionState.RolledBack);
        
        _context.TransactionEntities.Update(transaction);
        await _context.SaveChangesAsync();

        return new Result<TransactionEntity>(transaction);
    }
}
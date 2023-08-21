using EventTriangleAPI.Consumer.Domain.Constants;
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
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<TransactionEntity>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }

        if (requester.UserRole != UserRole.Admin)
        {
            return new Result<TransactionEntity>(new ConflictError(ResponseMessages.RequesterIsNotAdmin));
        }
        
        var transaction = await _context.TransactionEntities
            .Include(x => x.FromUser)
            .ThenInclude(x => x.Wallet)
            .Include(x => x.ToUser)
            .ThenInclude(x => x.Wallet)
            .FirstOrDefaultAsync(x => x.Id == command.TransactionId);

        if (transaction == null)
        {
            return new Result<TransactionEntity>(new DbEntityNotFoundError(ResponseMessages.TransactionTicketNotFound));
        }
        
        transaction.UpdateTransactionState(TransactionState.RolledBack);
        transaction.FromUser.Wallet.UpdateBalance(transaction.FromUser.Wallet.Balance + transaction.Amount);
        transaction.ToUser.Wallet.UpdateBalance(transaction.FromUser.Wallet.Balance + transaction.Amount);
        
        _context.TransactionEntities.Update(transaction);
        await _context.SaveChangesAsync();

        return new Result<TransactionEntity>(transaction);
    }
}
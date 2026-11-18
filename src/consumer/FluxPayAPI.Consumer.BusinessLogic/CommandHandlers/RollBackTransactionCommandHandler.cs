using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.Domain.Constants;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Enums;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;

public class RollBackTransactionCommandHandler : ICommandHandler<RollBackTransactionCommand, TransactionDto>
{
    private readonly DatabaseContext _context;

    public RollBackTransactionCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<TransactionDto, Error>> HandleAsync(RollBackTransactionCommand command)
    {
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<TransactionDto>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }

        if (requester.UserRole != UserRole.Admin)
        {
            return new Result<TransactionDto>(new ConflictError(ResponseMessages.RequesterIsNotAdmin));
        }
        
        var transaction = await _context.TransactionEntities
            .Include(x => x.FromUser)
            .ThenInclude(x => x.Wallet)
            .Include(x => x.ToUser)
            .ThenInclude(x => x.Wallet)
            .FirstOrDefaultAsync(x => x.Id == command.TransactionId);

        if (transaction == null)
        {
            return new Result<TransactionDto>(new DbEntityNotFoundError(ResponseMessages.TransactionTicketNotFound));
        }
        
        if (transaction.TransactionState == TransactionState.RolledBack)
        {
            return new Result<TransactionDto>(new ConflictError(ResponseMessages.TransactionHasAlreadyBeenRolledBack));
        }
        
        transaction.UpdateTransactionState(TransactionState.RolledBack);
        transaction.FromUser.Wallet.UpdateBalance(transaction.FromUser.Wallet.Balance + transaction.Amount);
        transaction.ToUser.Wallet.UpdateBalance(transaction.FromUser.Wallet.Balance + transaction.Amount);
        
        _context.TransactionEntities.Update(transaction);
        await _context.SaveChangesAsync();

        var transactionDto = new TransactionDto(
            transaction.Id,
            transaction.FromUserId,
            transaction.ToUserId,
            transaction.Amount,
            transaction.TransactionState,
            transaction.TransactionType,
            transaction.CreatedAt);
        
        return new Result<TransactionDto>(transactionDto);
    }
}
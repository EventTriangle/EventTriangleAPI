using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.Domain.Constants;
using FluxPayAPI.Consumer.Domain.Entities;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Enums;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;

public class CreateTransactionUserToUserCommandHandler : ICommandHandler<CreateTransactionUserToUserCommand, TransactionDto>
{
    private readonly DatabaseContext _context;

    public CreateTransactionUserToUserCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<TransactionDto, Error>> HandleAsync(CreateTransactionUserToUserCommand command)
    {
        var requester = await _context.UserEntities
            .Include(x => x.Wallet)
            .FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<TransactionDto>(new DbEntityNotFoundError(ResponseMessages.UserNotFound));
        }
        
        if (requester.UserStatus == UserStatus.Suspended)
        {
            return new Result<TransactionDto>(new ConflictError(ResponseMessages.RequesterIsSuspended));
        }
        
        var toUser = await _context.UserEntities
            .Include(x => x.Wallet)
            .FirstOrDefaultAsync(x => x.Id == command.ToUserId);

        if (toUser == null)
        {
            return new Result<TransactionDto>(new DbEntityNotFoundError(ResponseMessages.UserNotFound));
        }

        if (requester.Wallet.Balance < command.Amount)
        {
            return new Result<TransactionDto>(new ConflictError(ResponseMessages.CannotTransferMoreMoneyThanYouHave));
        }
        
        var transaction = new TransactionEntity(
            command.RequesterId, 
            command.ToUserId, 
            command.Amount, 
            TransactionType.FromUserToUser,
            command.CreatedAt);
        
        requester.Wallet.UpdateBalance(requester.Wallet.Balance - command.Amount);
        toUser.Wallet.UpdateBalance(toUser.Wallet.Balance + command.Amount);

        _context.TransactionEntities.Add(transaction);
        _context.WalletEntities.Update(requester.Wallet);
        _context.WalletEntities.Update(toUser.Wallet);

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
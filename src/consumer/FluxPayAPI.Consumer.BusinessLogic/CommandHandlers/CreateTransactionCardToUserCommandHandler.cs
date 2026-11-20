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

public class CreateTransactionCardToUserCommandHandler : ICommandHandler<CreateTransactionCardToUserCommand, TransactionDto>
{
    private readonly DatabaseContext _context;

    public CreateTransactionCardToUserCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<TransactionDto, Error>> HandleAsync(CreateTransactionCardToUserCommand command)
    {
        var requester = await _context.UserEntities
            .Include(x => x.Wallet)
            .FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<TransactionDto>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }

        if (requester.UserStatus == UserStatus.Suspended)
        {
            return new Result<TransactionDto>(new ConflictError(ResponseMessages.RequesterIsSuspended));
        }

        var creditCard = await _context.CreditCardEntities.FirstOrDefaultAsync(x => x.Id == command.CreditCardId && x.UserId == command.RequesterId);
        
        if (creditCard == null)
        {
            return new Result<TransactionDto>(new DbEntityNotFoundError(ResponseMessages.CreditCardNotFound));
        }
        
        var transaction = new TransactionEntity(
            command.RequesterId, 
            command.RequesterId, 
            command.Amount, 
            TransactionType.FromCardToUser,
            command.CreatedAt);

        requester.Wallet.UpdateBalance(requester.Wallet.Balance + command.Amount);
        
        _context.TransactionEntities.Add(transaction);
        _context.WalletEntities.Update(requester.Wallet);
        
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
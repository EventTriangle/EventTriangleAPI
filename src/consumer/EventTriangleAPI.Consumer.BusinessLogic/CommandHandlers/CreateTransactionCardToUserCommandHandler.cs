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

public class CreateTransactionCardToUserCommandHandler : ICommandHandler<CreateTransactionCardToUserCommand, TransactionEntity>
{
    private readonly DatabaseContext _context;

    public CreateTransactionCardToUserCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<TransactionEntity, Error>> HandleAsync(CreateTransactionCardToUserCommand command)
    {
        var requester = await _context.UserEntities
            .Include(x => x.Wallet)
            .FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<TransactionEntity>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }

        var creditCard = await _context.CreditCardEntities.FirstOrDefaultAsync(x => x.Id == command.CreditCardId);
        
        if (creditCard == null)
        {
            return new Result<TransactionEntity>(new DbEntityNotFoundError(ResponseMessages.CreditCardNotFound));
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

        return new Result<TransactionEntity>(transaction);
    }
}
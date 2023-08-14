using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public class CreateTransactionUserToUserCommandHandler : ICommandHandler<CreateTransactionUserToUserCommand, TransactionEntity>
{
    private readonly DatabaseContext _context;

    public CreateTransactionUserToUserCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<TransactionEntity, Error>> HandleAsync(CreateTransactionUserToUserCommand command)
    {
        var fromUser = await _context.UserEntities
            .Include(x => x.Wallet)
            .FirstOrDefaultAsync(x => x.Id == command.FromUserId);

        if (fromUser == null)
        {
            return new Result<TransactionEntity>(new DbEntityNotFoundError("User not found"));
        }
        
        var toUser = await _context.UserEntities
            .Include(x => x.Wallet)
            .FirstOrDefaultAsync(x => x.Id == command.ToUserId);

        if (toUser == null)
        {
            return new Result<TransactionEntity>(new DbEntityNotFoundError("The user to whom you want to transfer money is not found"));
        }

        if (fromUser.Wallet.Balance < command.Amount)
        {
            return new Result<TransactionEntity>(new ConflictError("You can't transfer more money than you have"));
        }
        
        var transaction = new TransactionEntity(
            command.FromUserId, 
            command.ToUserId, 
            command.Amount, 
            TransactionType.FromUserToUser);
        
        fromUser.Wallet.UpdateBalance(fromUser.Wallet.Balance - command.Amount);
        toUser.Wallet.UpdateBalance(toUser.Wallet.Balance + command.Amount);

        _context.TransactionEntities.Add(transaction);
        _context.WalletEntities.Update(fromUser.Wallet);
        _context.WalletEntities.Update(toUser.Wallet);

        await _context.SaveChangesAsync();

        return new Result<TransactionEntity>(transaction);
    }
}
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
        var user = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.ToUserId);

        if (user == null)
        {
            return new Result<TransactionEntity>(new DbEntityNotFoundError("User not found"));
        }

        var creditCard = await _context.CreditCardEntities.FirstOrDefaultAsync(x => x.Id == command.CreditCardId);
        
        if (creditCard == null)
        {
            return new Result<TransactionEntity>(new DbEntityNotFoundError("Credit card not found"));
        }
        
        var transaction = new TransactionEntity(
            command.ToUserId, 
            command.ToUserId, 
            command.Amount, 
            TransactionType.FromCardToUser);

        _context.TransactionEntities.Add(transaction);
        await _context.SaveChangesAsync();

        return new Result<TransactionEntity>(transaction);
    }
}
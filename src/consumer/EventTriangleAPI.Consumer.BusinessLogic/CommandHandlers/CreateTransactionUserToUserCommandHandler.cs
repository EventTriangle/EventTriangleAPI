using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

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
        var transaction = new TransactionEntity(
            command.FromUserId, 
            command.ToUserId, 
            command.Amount, 
            TransactionType.FromUserToUser);

        _context.TransactionEntities.Add(transaction);
        await _context.SaveChangesAsync();

        return new Result<TransactionEntity>(transaction);
    }
}
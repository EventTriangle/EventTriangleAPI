using EventTriangleAPI.Consumer.BusinessLogic.Models;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public class GetTransactionsByUserIdQueryHandler : ICommandHandler<GetTransactionsByUserIdQuery, List<TransactionDto>>
{
    private readonly DatabaseContext _context;

    public GetTransactionsByUserIdQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<List<TransactionDto>, Error>> HandleAsync(GetTransactionsByUserIdQuery command)
    {
        var isRequesterAdmin = await _context.UserEntities
            .AnyAsync(x => x.Id == command.RequesterId && x.UserRole == UserRole.Admin);

        if (!isRequesterAdmin)
        {
            return new Result<List<TransactionDto>>(new ConflictError(ResponseMessages.RequesterIsNotAdmin));
        }

        var isUserExisting = await _context.UserEntities.AnyAsync(x => x.Id == command.UserId);

        if (!isUserExisting)
        {
            return new Result<List<TransactionDto>>(new DbEntityNotFoundError(ResponseMessages.UserNotFound));
        }
        
        var transactions = await _context.TransactionEntities
            .OrderByDescending(x => x.CreatedAt)
            .Where(x => x.FromUserId == command.UserId || x.ToUserId == command.UserId)
            .Where(x => x.CreatedAt < command.FromDateTime)
            .Take(command.Limit)
            .Select(x => new TransactionDto(
                x.Id,
                x.FromUserId,
                x.ToUserId,
                x.Amount,
                x.TransactionState,
                x.TransactionType,
                x.CreatedAt)
            )
            .ToListAsync();

        return new Result<List<TransactionDto>>(transactions);
    }
}
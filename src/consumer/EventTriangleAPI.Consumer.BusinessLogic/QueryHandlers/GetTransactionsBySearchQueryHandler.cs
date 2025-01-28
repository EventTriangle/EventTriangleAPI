using EventTriangleAPI.Consumer.BusinessLogic.Models;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public class GetTransactionsBySearchQueryHandler : ICommandHandler<GetTransactionsBySearchQuery, List<TransactionDto>>
{
    private readonly DatabaseContext _context;

    public GetTransactionsBySearchQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<List<TransactionDto>, Error>> HandleAsync(GetTransactionsBySearchQuery command)
    {
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<List<TransactionDto>>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }

        var transactions = await _context.TransactionEntities
            .OrderByDescending(x => x.CreatedAt)
            .Where(x => x.FromUserId == command.RequesterId || x.ToUserId == command.RequesterId)
            .Where(x => x.CreatedAt < command.FromDateTime)
            .Where(x => EF.Functions.Like(x.Id.ToString(), $"%{command.SearchText}%"))
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
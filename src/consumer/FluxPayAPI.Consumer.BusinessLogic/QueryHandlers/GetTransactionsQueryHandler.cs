using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.Domain.Constants;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;

public class GetTransactionsQueryHandler : ICommandHandler<GetTransactionsQuery, List<TransactionDto>>
{
    private readonly DatabaseContext _context;

    public GetTransactionsQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<List<TransactionDto>, Error>> HandleAsync(GetTransactionsQuery command)
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
using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.Domain.Constants;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;

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

        var cursorQuery = _context.TransactionEntities.AsQueryable();
        if (command.CursorId is Guid cursorId)
        {
            cursorQuery = cursorQuery.Where(x => EF.Functions.LessThan(
                ValueTuple.Create(x.CreatedAt, x.Id), ValueTuple.Create(command.FromDateTime, cursorId)));
        }
        else
        {
            cursorQuery = cursorQuery.Where(x => x.CreatedAt < command.FromDateTime);
        }

        var transactions = await cursorQuery
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Where(x => x.FromUserId == command.RequesterId || x.ToUserId == command.RequesterId)
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
using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.Domain.Constants;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Enums;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;

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
            .Where(x => x.FromUserId == command.UserId || x.ToUserId == command.UserId)
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
using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.Domain.Constants;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;

public class GetSupportTicketsQueryHandler : ICommandHandler<GetSupportsTicketsQuery, List<SupportTicketDto>>
{
    private readonly DatabaseContext _context;

    public GetSupportTicketsQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<List<SupportTicketDto>, Error>> HandleAsync(GetSupportsTicketsQuery command)
    {
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<List<SupportTicketDto>>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }

        var supportTickets = await _context.SupportTicketEntities
            .OrderByDescending(x => x.CreatedAt)
            .Where(x => x.UserId == requester.Id)
            .Where(x => x.CreatedAt < command.FromDateTime)
            .Select(x => new SupportTicketDto(
                x.Id,
                x.TransactionId,
                x.UserId,
                x.WalletId,
                x.TicketReason,
                x.TicketJustification,
                x.TicketStatus,
                x.CreatedAt)
            )
            .Take(command.Limit)
            .ToListAsync();

        return new Result<List<SupportTicketDto>>(supportTickets);
    }
}
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

public class GetTicketsQueryHandler : ICommandHandler<GetTicketsQuery, List<SupportTicketDto>>
{
    private readonly DatabaseContext _context;

    public GetTicketsQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<List<SupportTicketDto>, Error>> HandleAsync(GetTicketsQuery command)
    {
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<List<SupportTicketDto>>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }

        if (requester.UserRole != UserRole.Admin)
        {
            return new Result<List<SupportTicketDto>>(new ConflictError(ResponseMessages.RequesterIsNotAdmin));
        }
        
        var supportTickets = await _context.SupportTicketEntities
            .OrderByDescending(x => x.CreatedAt)
            .Where(x => x.CreatedAt < command.FromDateTime)
            .Where(x => x.TicketStatus == TicketStatus.Open)
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
using EventTriangleAPI.Consumer.BusinessLogic.Models;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

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
                x.UserId,
                x.WalletId,
                x.TicketReason,
                x.TicketJustification,
                x.TicketStatus)
            )
            .Take(command.Limit)
            .ToListAsync();

        return new Result<List<SupportTicketDto>>(supportTickets);
    }
}
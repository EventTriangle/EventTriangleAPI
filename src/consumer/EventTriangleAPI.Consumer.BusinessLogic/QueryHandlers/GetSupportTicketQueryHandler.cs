using EventTriangleAPI.Consumer.BusinessLogic.Models;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public class GetSupportTicketQueryHandler : ICommandHandler<GetSupportTicketQuery, List<SupportTicketDto>>
{
    private readonly DatabaseContext _context;

    public GetSupportTicketQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<List<SupportTicketDto>, Error>> HandleAsync(GetSupportTicketQuery command)
    {
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<List<SupportTicketDto>>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }

        var supportTickets = await _context.SupportTicketEntities
            .Where(x => x.UserId == requester.Id)
            .Select(x => new SupportTicketDto
            {
                Id = x.Id,
                UserId = x.UserId,
                WalletId = x.WalletId,
                TicketReason = x.TicketReason,
                TicketJustification = x.TicketJustification,
                TicketStatus = x.TicketStatus
            })
            .ToListAsync();

        return new Result<List<SupportTicketDto>>(supportTickets);
    }
}
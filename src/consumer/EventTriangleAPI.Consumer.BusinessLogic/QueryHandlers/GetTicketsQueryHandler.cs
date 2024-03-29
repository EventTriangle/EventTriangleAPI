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
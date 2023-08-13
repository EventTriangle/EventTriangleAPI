using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public class ResolveSupportTicketCommandHandler : ICommandHandler<ResolveSupportTicketCommand, SupportTicketEntity>
{
    private readonly DatabaseContext _context;

    public ResolveSupportTicketCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<SupportTicketEntity, Error>> HandleAsync(ResolveSupportTicketCommand command)
    {
        var supportTicket = await _context.SupportTicketEntities
            .FirstOrDefaultAsync(x => x.Id == command.TicketId);

        if (supportTicket == null)
        {
            return new Result<SupportTicketEntity>(new DbEntityNotFoundError("Support ticket not found"));
        }
        
        supportTicket.UpdateTicketJustification(command.TicketJustification);
        supportTicket.UpdateTicketStatus(TicketStatus.Resolved);

        _context.SupportTicketEntities.Update(supportTicket);
        await _context.SaveChangesAsync();

        return new Result<SupportTicketEntity>(supportTicket);
    }
}
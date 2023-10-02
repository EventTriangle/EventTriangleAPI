using EventTriangleAPI.Consumer.BusinessLogic.Models;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public class ResolveSupportTicketCommandHandler : ICommandHandler<ResolveSupportTicketCommand, SupportTicketDto>
{
    private readonly DatabaseContext _context;

    public ResolveSupportTicketCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<SupportTicketDto, Error>> HandleAsync(ResolveSupportTicketCommand command)
    {
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<SupportTicketDto>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }

        if (requester.UserRole != UserRole.Admin)
        {
            return new Result<SupportTicketDto>(new ConflictError(ResponseMessages.RequesterIsNotAdmin));
        }
        
        var supportTicket = await _context.SupportTicketEntities
            .FirstOrDefaultAsync(x => x.Id == command.TicketId);

        if (supportTicket == null)
        {
            return new Result<SupportTicketDto>(new DbEntityNotFoundError(ResponseMessages.SupportTicketNotFound));
        }
        
        supportTicket.UpdateTicketJustification(command.TicketJustification);
        supportTicket.UpdateTicketStatus(TicketStatus.Resolved);

        _context.SupportTicketEntities.Update(supportTicket);
        await _context.SaveChangesAsync();

        var supportTicketDto = new SupportTicketDto(
            supportTicket.Id,
            supportTicket.TransactionId,
            supportTicket.UserId,
            supportTicket.WalletId,
            supportTicket.TicketReason,
            supportTicket.TicketJustification,
            supportTicket.TicketStatus);
        
        return new Result<SupportTicketDto>(supportTicketDto);
    }
}
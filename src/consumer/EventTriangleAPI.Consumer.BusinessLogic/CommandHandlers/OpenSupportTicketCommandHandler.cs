using EventTriangleAPI.Consumer.BusinessLogic.Models;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public class OpenSupportTicketCommandHandler : ICommandHandler<OpenSupportTicketCommand, SupportTicketDto>
{
    private readonly DatabaseContext _context;

    public OpenSupportTicketCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<SupportTicketDto, Error>> HandleAsync(OpenSupportTicketCommand command)
    {
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<SupportTicketDto>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }
        
        var wallet = await _context.WalletEntities.FirstOrDefaultAsync(x => x.Id == command.WalletId);

        if (wallet == null)
        {
            return new Result<SupportTicketDto>(new DbEntityNotFoundError(ResponseMessages.WalletNotFound));
        }

        var transaction = await _context.TransactionEntities
            .Where(x => x.FromUserId == command.RequesterId || x.ToUserId == command.RequesterId)
            .FirstOrDefaultAsync(x => x.Id == command.TransactionId);

        if (transaction == null)
        {
            return new Result<SupportTicketDto>(new DbEntityNotFoundError(ResponseMessages.TransactionNotFound));
        }

        var isThereSupportTicket = await _context.SupportTicketEntities.AnyAsync(x => x.TransactionId == command.TransactionId);

        if (isThereSupportTicket)
        {
            return new Result<SupportTicketDto>(new ConflictError(ResponseMessages.SupportTicketAlreadyExists));
        }
        
        var supportTicket = new SupportTicketEntity(
            command.RequesterId,
            command.WalletId,
            command.TransactionId,
            command.TicketReason,
            command.CreatedAt);

        _context.SupportTicketEntities.Add(supportTicket);
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
using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public class OpenSupportTicketCommandHandler : ICommandHandler<OpenSupportTicketCommand, SupportTicketEntity>
{
    private readonly DatabaseContext _context;

    public OpenSupportTicketCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<SupportTicketEntity, Error>> HandleAsync(OpenSupportTicketCommand command)
    {
        var user = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.UserId);

        if (user == null)
        {
            return new Result<SupportTicketEntity>(new DbEntityNotFoundError("User not found"));
        }
        
        var wallet = await _context.WalletEntities.FirstOrDefaultAsync(x => x.Id == command.WalletId);

        if (wallet == null)
        {
            return new Result<SupportTicketEntity>(new DbEntityNotFoundError("Wallet not found"));
        }
        
        var supportTicket = new SupportTicketEntity(command.UserId, command.WalletId, command.TicketReason);

        _context.SupportTicketEntities.Add(supportTicket);
        await _context.SaveChangesAsync();

        return new Result<SupportTicketEntity>(supportTicket);
    }
}
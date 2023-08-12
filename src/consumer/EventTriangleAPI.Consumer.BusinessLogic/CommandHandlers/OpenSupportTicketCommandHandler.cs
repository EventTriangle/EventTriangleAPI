using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

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
        var supportTicket = new SupportTicketEntity(command.UserId, command.WalletId, command.TicketReason);

        _context.SupportTicketEntities.Add(supportTicket);
        await _context.SaveChangesAsync();

        return new Result<SupportTicketEntity>(supportTicket);
    }
}
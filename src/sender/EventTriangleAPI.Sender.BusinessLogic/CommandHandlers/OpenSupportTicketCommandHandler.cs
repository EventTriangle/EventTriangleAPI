using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class OpenSupportTicketCommandHandler : ICommandHandler<OpenSupportTicketBody, string>
{
    private readonly DatabaseContext _context;

    public OpenSupportTicketCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(ICommand<OpenSupportTicketBody> command)
    {
        var supportTicketOpenedEvent = new SupportTicketOpenedEvent(
            command.Body.RequesterId,
            command.Body.Username,
            command.Body.WalletId,
            command.Body.TicketReason);

        _context.SupportTicketOpenedEvents.Add(supportTicketOpenedEvent);
        await _context.SaveChangesAsync();

        return new Result<string>("");
    }
}
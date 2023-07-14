using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class ResolveSupportTicketCommandHandler : ICommandHandler<ResolveSupportTicketBody, string>
{
    private readonly DatabaseContext _context;

    public ResolveSupportTicketCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(ICommand<ResolveSupportTicketBody> command)
    {
        var supportTicketResolvedEvent = new SupportTicketResolvedEvent(
            command.Body.TicketId, 
            command.Body.TicketJustification);

        _context.SupportTicketResolvedEvents.Add(supportTicketResolvedEvent);
        await _context.SaveChangesAsync();
        
        new MockOrder().Send(supportTicketResolvedEvent);

        return new Result<string>("");
    }
}
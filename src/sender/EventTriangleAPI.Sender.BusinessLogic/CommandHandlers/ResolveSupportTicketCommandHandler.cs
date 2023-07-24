using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.Domain.Events;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class ResolveSupportTicketCommandHandler : ICommandHandler<ResolveSupportTicketCommand, SupportTicketResolvedEvent>
{
    private readonly DatabaseContext _context;

    public ResolveSupportTicketCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<SupportTicketResolvedEvent, Error>> HandleAsync(ResolveSupportTicketCommand command)
    {
        var supportTicketResolvedEvent = new SupportTicketResolvedEvent(
            command.TicketId, 
            command.TicketJustification);

        _context.SupportTicketResolvedEvents.Add(supportTicketResolvedEvent);
        await _context.SaveChangesAsync();
        
        new MockOrder().Send(supportTicketResolvedEvent);

        return new Result<SupportTicketResolvedEvent>(supportTicketResolvedEvent);
    }
}
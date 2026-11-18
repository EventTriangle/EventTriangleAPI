using FluxPayAPI.Sender.Domain.Entities;
using FluxPayAPI.Sender.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public class ResolveSupportTicketCommandHandler : ICommandHandler<ResolveSupportTicketCommand, SupportTicketResolvedEvent>
{
    private readonly DatabaseContext _context;
    private readonly IPublishEndpoint _client;

    public ResolveSupportTicketCommandHandler(DatabaseContext context, IPublishEndpoint client)
    {
        _context = context;
        _client = client;
    }

    public async Task<IResult<SupportTicketResolvedEvent, Error>> HandleAsync(ResolveSupportTicketCommand command)
    {
        var supportTicketResolvedEvent = new SupportTicketResolvedEvent(
            command.RequesterId,
            command.TicketId, 
            command.TicketJustification);

        _context.SupportTicketResolvedEvents.Add(supportTicketResolvedEvent);
        await _context.SaveChangesAsync();
        
        var _ = _client.Publish(supportTicketResolvedEvent.CreateEventMessage());

        return new Result<SupportTicketResolvedEvent>(supportTicketResolvedEvent);
    }
}
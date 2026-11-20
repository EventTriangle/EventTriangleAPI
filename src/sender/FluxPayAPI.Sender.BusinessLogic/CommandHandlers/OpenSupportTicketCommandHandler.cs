using FluxPayAPI.Sender.Domain.Entities;
using FluxPayAPI.Sender.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public class OpenSupportTicketCommandHandler : ICommandHandler<OpenSupportTicketCommand, SupportTicketOpenedEvent>
{
    private readonly DatabaseContext _context;
    private readonly IPublishEndpoint _client;
    
    public OpenSupportTicketCommandHandler(DatabaseContext context, IPublishEndpoint client)
    {
        _context = context;
        _client = client;
    }

    public async Task<IResult<SupportTicketOpenedEvent, Error>> HandleAsync(OpenSupportTicketCommand command)
    {
        var supportTicketOpenedEvent = new SupportTicketOpenedEvent(
            command.RequesterId,
            command.WalletId,
            command.TransactionId,
            command.TicketReason);

        _context.SupportTicketOpenedEvents.Add(supportTicketOpenedEvent);
        await _client.Publish(supportTicketOpenedEvent.CreateEventMessage());
        await _context.SaveChangesAsync();
        
        return new Result<SupportTicketOpenedEvent>(supportTicketOpenedEvent);
    }
}
using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

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
        await _context.SaveChangesAsync();

        var _ = _client.Publish(supportTicketOpenedEvent.CreateEventMessage());
        
        return new Result<SupportTicketOpenedEvent>(supportTicketOpenedEvent);
    }
}
using FluxPayAPI.Sender.Domain.Entities;
using FluxPayAPI.Sender.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public class DeleteContactCommandHandler : ICommandHandler<DeleteContactCommand, ContactDeletedEvent>
{
    private readonly DatabaseContext _context;
    private readonly IPublishEndpoint _client;

    public DeleteContactCommandHandler(DatabaseContext context, IPublishEndpoint client)
    {
        _context = context;
        _client = client;
    }

    public async Task<IResult<ContactDeletedEvent, Error>> HandleAsync(DeleteContactCommand command)
    {
        var contactDeletedEvent = new ContactDeletedEvent(command.RequesterId, command.ContactId);

        _context.ContactDeletedEvents.Add(contactDeletedEvent);
        await _client.Publish(contactDeletedEvent.CreateEventMessage());
        await _context.SaveChangesAsync();
        
        return new Result<ContactDeletedEvent>(contactDeletedEvent);
    }
}
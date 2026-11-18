using FluxPayAPI.Sender.Domain.Entities;
using FluxPayAPI.Sender.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public class AddContactCommandHandler : ICommandHandler<AddContactCommand, ContactCreatedEvent>
{
    private readonly DatabaseContext _context;
    private readonly IPublishEndpoint _client;
    
    public AddContactCommandHandler(DatabaseContext context, IPublishEndpoint client)
    {
        _context = context;
        _client = client;
    }

    public async Task<IResult<ContactCreatedEvent, Error>> HandleAsync(AddContactCommand command)
    {
        var contactCreatedEvent = new ContactCreatedEvent(command.RequesterId, command.ContactId);

        _context.ContactCreatedEvents.Add(contactCreatedEvent);
        await _context.SaveChangesAsync();
        
        var _ = _client.Publish(contactCreatedEvent.CreateEventMessage());
    
        return new Result<ContactCreatedEvent>(contactCreatedEvent);
    }
}
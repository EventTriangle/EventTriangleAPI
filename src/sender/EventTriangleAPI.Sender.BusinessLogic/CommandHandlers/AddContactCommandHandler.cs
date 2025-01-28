using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

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
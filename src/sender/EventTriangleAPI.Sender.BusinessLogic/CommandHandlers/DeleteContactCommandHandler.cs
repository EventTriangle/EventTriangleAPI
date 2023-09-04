using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

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
        await _context.SaveChangesAsync();
        
        var _ = _client.Publish(contactDeletedEvent.CreateEventMessage());
        
        return new Result<ContactDeletedEvent>(contactDeletedEvent);
    }
}
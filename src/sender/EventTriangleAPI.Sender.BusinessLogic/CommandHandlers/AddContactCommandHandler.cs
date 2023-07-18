using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class AddContactCommandHandler : ICommandHandler<AddContactBody, ContactCreatedEvent>
{
    private readonly DatabaseContext _context;

    public AddContactCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<ContactCreatedEvent, Error>> HandleAsync(ICommand<AddContactBody> command)
    {
        var contactCreatedEvent = new ContactCreatedEvent(command.Body.UserId, command.Body.ContactId);

        _context.ContactCreatedEvents.Add(contactCreatedEvent);
        await _context.SaveChangesAsync();
        
        new MockOrder().Send(contactCreatedEvent);

        return new Result<ContactCreatedEvent>(contactCreatedEvent);
    }
}
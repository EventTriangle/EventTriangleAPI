using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.Domain.Entities;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class DeleteContactCommandHandler : ICommandHandler<DeleteContactCommand, ContactDeletedEvent>
{
    private readonly DatabaseContext _context;

    public DeleteContactCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<ContactDeletedEvent, Error>> HandleAsync(DeleteContactCommand command)
    {
        var contactDeletedEvent = new ContactDeletedEvent(command.UserId, command.ContactId);

        _context.ContactDeletedEvents.Add(contactDeletedEvent);
        await _context.SaveChangesAsync();
        
        new MockOrder().Send(contactDeletedEvent);

        return new Result<ContactDeletedEvent>(contactDeletedEvent);
    }
}
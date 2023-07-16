using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class DeleteContactCommandHandler : ICommandHandler<DeleteContactBody, string>
{
    private readonly DatabaseContext _context;

    public DeleteContactCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(ICommand<DeleteContactBody> command)
    {
        var contactDeletedEvent = new ContactDeletedEvent(command.Body.UserId, command.Body.ContactId);

        _context.ContactDeletedEvents.Add(contactDeletedEvent);
        await _context.SaveChangesAsync();
        
        new MockOrder().Send(contactDeletedEvent);

        return new Result<string>("");
    }
}
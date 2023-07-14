using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class NotSuspendUserCommandHandler : ICommandHandler<NotSuspendUserBody, string>
{
    private readonly DatabaseContext _context;

    public NotSuspendUserCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(ICommand<NotSuspendUserBody> command)
    {
        var userNotSuspendedEvent = new UserNotSuspendedEvent(command.Body.UserId);

        _context.UserNotSuspendedEvents.Add(userNotSuspendedEvent);
        await _context.SaveChangesAsync();
        
        new MockOrder().Send(userNotSuspendedEvent);
        
        return new Result<string>("");
    }
}
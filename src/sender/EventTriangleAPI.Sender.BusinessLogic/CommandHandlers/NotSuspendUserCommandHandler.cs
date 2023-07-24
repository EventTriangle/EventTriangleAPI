using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.Domain.Events;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class NotSuspendUserCommandHandler : ICommandHandler<NotSuspendUserCommand, UserNotSuspendedEvent>
{
    private readonly DatabaseContext _context;

    public NotSuspendUserCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<UserNotSuspendedEvent, Error>> HandleAsync(NotSuspendUserCommand command)
    {
        var userNotSuspendedEvent = new UserNotSuspendedEvent(command.UserId);

        _context.UserNotSuspendedEvents.Add(userNotSuspendedEvent);
        await _context.SaveChangesAsync();
        
        new MockOrder().Send(userNotSuspendedEvent);
        
        return new Result<UserNotSuspendedEvent>(userNotSuspendedEvent);
    }
}
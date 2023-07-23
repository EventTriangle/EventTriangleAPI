using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.Domain.Entities;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class SuspendUserCommandHandler : ICommandHandler<SuspendUserCommand, UserSuspendedEvent>
{
    private readonly DatabaseContext _context;

    public SuspendUserCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<UserSuspendedEvent, Error>> HandleAsync(SuspendUserCommand command)
    {
        var userSuspendedEvent = new UserSuspendedEvent(command.UserId);

        _context.UserSuspendedEvents.Add(userSuspendedEvent);
        await _context.SaveChangesAsync();

        new MockOrder().Send(userSuspendedEvent);
        
        return new Result<UserSuspendedEvent>(userSuspendedEvent);
    }
}
using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class SuspendUserCommandHandler : ICommandHandler<SuspendUserBody, string>
{
    private readonly DatabaseContext _context;

    public SuspendUserCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(ICommand<SuspendUserBody> command)
    {
        var userSuspendedEvent = new UserSuspendedEvent(command.Body.UserId);

        _context.UserSuspendedEvents.Add(userSuspendedEvent);
        await _context.SaveChangesAsync();

        new MockOrder().Send(userSuspendedEvent);
        
        return new Result<string>("");
    }
}
using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class NotSuspendUserCommandHandler : ICommandHandler<NotSuspendUserCommand, UserNotSuspendedEvent>
{
    private readonly DatabaseContext _context;
    private readonly IPublishEndpoint _client;
    
    public NotSuspendUserCommandHandler(DatabaseContext context, IPublishEndpoint client)
    {
        _context = context;
        _client = client;
    }

    public async Task<IResult<UserNotSuspendedEvent, Error>> HandleAsync(NotSuspendUserCommand command)
    {
        var userNotSuspendedEvent = new UserNotSuspendedEvent(command.RequesterId, command.UserId);

        _context.UserNotSuspendedEvents.Add(userNotSuspendedEvent);
        await _context.SaveChangesAsync();
        
        var _ = _client.Publish(userNotSuspendedEvent.CreateEventMessage());
        
        return new Result<UserNotSuspendedEvent>(userNotSuspendedEvent);
    }
}
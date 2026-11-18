using FluxPayAPI.Sender.Domain.Entities;
using FluxPayAPI.Sender.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

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
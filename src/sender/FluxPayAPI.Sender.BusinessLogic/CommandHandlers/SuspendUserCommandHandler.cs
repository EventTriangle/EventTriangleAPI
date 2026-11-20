using FluxPayAPI.Sender.Domain.Entities;
using FluxPayAPI.Sender.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public class SuspendUserCommandHandler : ICommandHandler<SuspendUserCommand, UserSuspendedEvent>
{
    private readonly DatabaseContext _context;
    private readonly IPublishEndpoint _client;

    public SuspendUserCommandHandler(DatabaseContext context, IPublishEndpoint client)
    {
        _context = context;
        _client = client;
    }

    public async Task<IResult<UserSuspendedEvent, Error>> HandleAsync(SuspendUserCommand command)
    {
        var userSuspendedEvent = new UserSuspendedEvent(command.RequesterId, command.UserId);

        _context.UserSuspendedEvents.Add(userSuspendedEvent);
        await _client.Publish(userSuspendedEvent.CreateEventMessage());
        await _context.SaveChangesAsync();
        
        return new Result<UserSuspendedEvent>(userSuspendedEvent);
    }
}
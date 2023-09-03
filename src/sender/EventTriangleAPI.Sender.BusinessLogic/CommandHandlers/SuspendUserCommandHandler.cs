using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

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
        await _context.SaveChangesAsync();

        var _ = _client.Publish(userSuspendedEvent.CreateEventMessage());
        
        return new Result<UserSuspendedEvent>(userSuspendedEvent);
    }
}
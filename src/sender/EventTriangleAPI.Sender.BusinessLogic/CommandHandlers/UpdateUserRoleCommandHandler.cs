using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class UpdateUserRoleCommandHandler : ICommandHandler<UpdateUserRoleCommand, UserRoleUpdatedEvent>
{
    private readonly DatabaseContext _context;
    private readonly IPublishEndpoint _client;

    public UpdateUserRoleCommandHandler(DatabaseContext context, IPublishEndpoint client)
    {
        _context = context;
        _client = client;
    }

    public async Task<IResult<UserRoleUpdatedEvent, Error>> HandleAsync(UpdateUserRoleCommand command)
    {
        var userRoleUpdatedEvent = new UserRoleUpdatedEvent(command.RequesterId, command.UserId, command.UserRole);

        _context.UserRoleUpdatedEvents.Add(userRoleUpdatedEvent);
        await _context.SaveChangesAsync();

        var _ = _client.Publish(userRoleUpdatedEvent.CreateEventMessage());
        
        return new Result<UserRoleUpdatedEvent>(userRoleUpdatedEvent);
    }
}
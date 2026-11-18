using FluxPayAPI.Sender.Domain.Entities;
using FluxPayAPI.Sender.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

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
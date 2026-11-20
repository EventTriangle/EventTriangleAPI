using FluxPayAPI.Sender.Domain.Entities;
using FluxPayAPI.Sender.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserCreatedEvent>
{
    private readonly DatabaseContext _context;
    private readonly IPublishEndpoint _client;

    public CreateUserCommandHandler(DatabaseContext context, IPublishEndpoint client)
    {
        _context = context;
        _client = client;
    }

    public async Task<IResult<UserCreatedEvent, Error>> HandleAsync(CreateUserCommand command)
    {
        var userCreatedEvent = new UserCreatedEvent(
            command.UserId,
            command.Email,
            command.UserRole, 
            command.UserStatus);
            
        _context.UserCreatedEvents.Add(userCreatedEvent);
        await _client.Publish(userCreatedEvent.CreateEventMessage());
        await _context.SaveChangesAsync();
        
        return new Result<UserCreatedEvent>(userCreatedEvent);
    }
}
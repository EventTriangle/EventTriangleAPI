using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

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
        await _context.SaveChangesAsync();

        var _ = _client.Publish(userCreatedEvent.CreateEventMessage());
        
        return new Result<UserCreatedEvent>(userCreatedEvent);
    }
}
using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class UpdateUserRoleCommandHandler : ICommandHandler<UpdateUserRoleBody, string>
{
    private readonly DatabaseContext _context;

    public UpdateUserRoleCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(ICommand<UpdateUserRoleBody> command)
    {
        var userRoleUpdatedEvent = new UserRoleUpdatedEvent(command.Body.UserId, command.Body.UserRole);

        _context.UserRoleUpdatedEvents.Add(userRoleUpdatedEvent);
        await _context.SaveChangesAsync();

        new MockOrder().Send(userRoleUpdatedEvent);
        
        return new Result<string>("");
    }
}
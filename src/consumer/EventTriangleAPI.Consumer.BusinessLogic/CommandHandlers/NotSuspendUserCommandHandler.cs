using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public class NotSuspendUserCommandHandler : ICommandHandler<NotSuspendUserCommand, UserEntity>
{
    private readonly DatabaseContext _context;

    public NotSuspendUserCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<UserEntity, Error>> HandleAsync(NotSuspendUserCommand command)
    {
        var user = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.UserId);

        if (user == null)
        {
            return new Result<UserEntity>(new DbEntityNotFoundError("User not found"));
        }
        
        user.UpdateUserStatus(UserStatus.Active);

        _context.UserEntities.Update(user);
        await _context.SaveChangesAsync();

        return new Result<UserEntity>(user);
    }
}
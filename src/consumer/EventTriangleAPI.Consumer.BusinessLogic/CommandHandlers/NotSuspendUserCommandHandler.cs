using EventTriangleAPI.Consumer.BusinessLogic.Models;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public class NotSuspendUserCommandHandler : ICommandHandler<NotSuspendUserCommand, UserDto>
{
    private readonly DatabaseContext _context;

    public NotSuspendUserCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<UserDto, Error>> HandleAsync(NotSuspendUserCommand command)
    {
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<UserDto>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }

        if (requester.UserRole != UserRole.Admin)
        {
            return new Result<UserDto>(new ConflictError(ResponseMessages.RequesterIsNotAdmin));
        }
        
        var user = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.UserId);

        if (user == null)
        {
            return new Result<UserDto>(new DbEntityNotFoundError(ResponseMessages.UserNotFound));
        }
        
        user.UpdateUserStatus(UserStatus.Active);

        _context.UserEntities.Update(user);
        await _context.SaveChangesAsync();

        var userDto = new UserDto(user.Id, user.Email, user.UserRole, user.UserStatus, user.WalletId, null);
        
        return new Result<UserDto>(userDto);
    }
}
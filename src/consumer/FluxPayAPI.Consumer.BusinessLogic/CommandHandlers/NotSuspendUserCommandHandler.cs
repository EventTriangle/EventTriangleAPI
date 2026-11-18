using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.Domain.Constants;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Enums;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;

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
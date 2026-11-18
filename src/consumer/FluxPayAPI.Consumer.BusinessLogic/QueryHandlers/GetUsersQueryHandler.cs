using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.Domain.Constants;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Enums;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;

public class GetUsersQueryHandler : ICommandHandler<GetUsersQuery, List<UserDto>>
{
    private readonly DatabaseContext _context;

    public GetUsersQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<List<UserDto>, Error>> HandleAsync(GetUsersQuery command)
    {
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<List<UserDto>>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }

        if (requester.UserRole != UserRole.Admin)
        {
            return new Result<List<UserDto>>(new ConflictError(ResponseMessages.RequesterIsNotAdmin));
        }

        if (command.Page < 1)
        {
            return new Result<List<UserDto>>(new BadRequestError(ResponseMessages.PageCannotBeLessThanOne));
        }
        
        var limit = command.Limit < 1 ? DefaultValueConstants.DefaultLimit : command.Limit; 
        
        var users = await _context.UserEntities
            .Include(x => x.Wallet)
            .Where(x => x.Id != requester.Id)
            .Where(x => x.UserRole != UserRole.Admin)
            .Select(x => new UserDto(
                x.Id,
                x.Email,
                x.UserRole,
                x.UserStatus,
                x.WalletId,
                new WalletDto(
                    x.WalletId,
                    x.Wallet.Balance,
                    x.Wallet.LastTransactionId))
            )
            .Skip((command.Page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return new Result<List<UserDto>>(users);
    }
}
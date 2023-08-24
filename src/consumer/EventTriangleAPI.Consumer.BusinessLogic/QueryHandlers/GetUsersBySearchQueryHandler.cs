using EventTriangleAPI.Consumer.BusinessLogic.Models;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public class GetUsersBySearchQueryHandler : ICommandHandler<GetUsersBySearchQuery, List<UserDto>>
{
    private readonly DatabaseContext _context;

    public GetUsersBySearchQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<List<UserDto>, Error>> HandleAsync(GetUsersBySearchQuery command)
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
            .Where(x => EF.Functions.Like(x.Email, $"%{command.Email}%"))
            .Select(x => 
                new UserDto(
                    x.Id, 
                    x.Email,
                    x.UserRole, 
                    x.UserStatus, 
                    new WalletDto(
                        x.WalletId, 
                        x.Wallet.Balance, 
                        x.Wallet.LastTransactionId)))
            .Skip((command.Page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return new Result<List<UserDto>>(users);
    }
}
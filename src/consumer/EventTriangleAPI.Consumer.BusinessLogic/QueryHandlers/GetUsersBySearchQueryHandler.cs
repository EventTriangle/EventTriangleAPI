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

public class GetUsersBySearchQueryHandler: ICommandHandler<GetUsersBySearchQuery, List<UserDto>>
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

        if (command.Limit < 1)
        {
            return new Result<List<UserDto>>(new BadRequestError(ResponseMessages.PageCannotBeLessThanZero));
        }
        
        var users = await _context.UserEntities
            .Where(x => EF.Functions.Like(x.Email, $"%{command.Email}%"))
            .Select(x => new UserDto 
            {
                Id = requester.Id,
                Email = requester.Email,
                UserRole = requester.UserRole,
                UserStatus = requester.UserStatus,
                Wallet = new WalletDto
                {
                    Id = requester.Wallet.Id,
                    Balance = requester.Wallet.Balance,
                    LastTransactionId = requester.Wallet.LastTransactionId
                }
            })
            .Skip((command.Page - 1) * command.Limit)
            .Take(command.Limit)
            .ToListAsync();

        return new Result<List<UserDto>>(users);
    }
}
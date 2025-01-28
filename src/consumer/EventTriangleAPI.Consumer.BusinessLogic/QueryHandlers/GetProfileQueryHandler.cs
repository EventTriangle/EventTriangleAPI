using EventTriangleAPI.Consumer.BusinessLogic.Models;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public class GetProfileQueryHandler : ICommandHandler<GetProfileQuery, UserDto>
{
    private readonly DatabaseContext _context;

    public GetProfileQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<UserDto, Error>> HandleAsync(GetProfileQuery command)
    {
        var requester = await _context.UserEntities
            .Include(x => x.Wallet)
            .FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<UserDto>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }

        var walletDto = new WalletDto(
            requester.Wallet.Id,
            requester.Wallet.Balance,
            requester.Wallet.LastTransactionId);

        var userDto = new UserDto(
            requester.Id,
            requester.Email,
            requester.UserRole,
            requester.UserStatus,
            requester.WalletId,
            walletDto);

        return new Result<UserDto>(userDto);
    }
}
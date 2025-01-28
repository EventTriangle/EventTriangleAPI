using EventTriangleAPI.Consumer.BusinessLogic.Models;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public class GetProfileByIdQueryHandler : ICommandHandler<GetProfileByIdQuery, UserDto>
{
    private readonly DatabaseContext _context;

    public GetProfileByIdQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<UserDto, Error>> HandleAsync(GetProfileByIdQuery command)
    {
        var user = await _context.UserEntities
            .Where(x => x.Id == command.UserId)
            .Select(x => new UserDto(
                x.Id,
                x.Email,
                x.UserRole,
                x.UserStatus,
                x.WalletId, 
                null)
            )
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return new Result<UserDto>(new DbEntityNotFoundError(ResponseMessages.UserNotFound));
        }

        return new Result<UserDto>(user);
    }
}
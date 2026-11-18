using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.Domain.Constants;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;

public class GetContactsBySearchQueryHandler : ICommandHandler<GetContactsBySearchQuery, List<ContactDto>>
{
    private readonly DatabaseContext _context;

    public GetContactsBySearchQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<List<ContactDto>, Error>> HandleAsync(GetContactsBySearchQuery command)
    {
        var requester = await _context.UserEntities
            .Include(x => x.Contacts)
            .FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<List<ContactDto>>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }

        var contactIdList = requester.Contacts.Select(x => x.ContactId).ToList();

        var users = await _context.UserEntities
            .Where(x => EF.Functions.Like(x.Email, $"%{command.Email}%"))
            .Where(x => !contactIdList.Contains(x.Id))
            .Where(x => x.Id != requester.Id)
            .Select(x => new ContactDto(
                requester.Id,
                x.Id,
                new UserDto(
                    x.Id,
                    x.Email,
                    x.UserRole,
                    x.UserStatus,
                    x.WalletId,
                    null)))
            .ToListAsync();

        return new Result<List<ContactDto>>(users);
    }
}
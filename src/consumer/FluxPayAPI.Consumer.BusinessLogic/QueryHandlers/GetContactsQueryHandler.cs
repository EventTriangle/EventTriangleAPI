using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.Domain.Constants;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;

public class GetContactsQueryHandler : ICommandHandler<GetContactsQuery, List<ContactDto>>
{
    private readonly DatabaseContext _context;

    public GetContactsQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<List<ContactDto>, Error>> HandleAsync(GetContactsQuery command)
    {
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<List<ContactDto>>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }

        var contacts = await _context.ContactEntities
            .Include(x => x.Contact)
            .Where(x => x.UserId == requester.Id)
            .Select(x => new ContactDto(
                x.UserId, 
                x.ContactId, 
                new UserDto(
                    x.Contact.Id,
                    x.Contact.Email,
                    x.Contact.UserRole,
                    x.Contact.UserStatus,
                    x.Contact.WalletId,
                    null))
            )
            .ToListAsync();

        return new Result<List<ContactDto>>(contacts);
    }
}
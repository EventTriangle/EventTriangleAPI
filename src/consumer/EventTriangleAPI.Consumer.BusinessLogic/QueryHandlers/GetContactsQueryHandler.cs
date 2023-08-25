using EventTriangleAPI.Consumer.BusinessLogic.Models;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

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
                    null))
            )
            .ToListAsync();

        return new Result<List<ContactDto>>(contacts);
    }
}
using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.Domain.Constants;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;

public class DeleteContactCommandHandler : ICommandHandler<DeleteContactCommand, ContactDto>
{
    private readonly DatabaseContext _context;

    public DeleteContactCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<ContactDto, Error>> HandleAsync(DeleteContactCommand command)
    {
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<ContactDto>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }
        
        var contact = await _context.ContactEntities
            .FirstOrDefaultAsync(x => x.UserId == command.RequesterId && x.ContactId == command.ContactId);

        if (contact == null)
        {
            return new Result<ContactDto>(new DbEntityNotFoundError(ResponseMessages.ContactNotFound));
        }

        _context.ContactEntities.Remove(contact);
        await _context.SaveChangesAsync();

        var contactDto = new ContactDto(contact.UserId, contact.ContactId, null);
        
        return new Result<ContactDto>(contactDto);
    }
}
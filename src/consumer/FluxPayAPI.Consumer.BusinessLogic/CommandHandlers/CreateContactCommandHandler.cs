using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.Domain.Constants;
using FluxPayAPI.Consumer.Domain.Entities;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;

public class CreateContactCommandHandler : ICommandHandler<CreateContactCommand, ContactDto>
{
    private readonly DatabaseContext _context;

    public CreateContactCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<ContactDto, Error>> HandleAsync(CreateContactCommand command)
    {
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<ContactDto>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }
        
        var user = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.ContactId);

        if (user == null)
        {
            return new Result<ContactDto>(new DbEntityNotFoundError(ResponseMessages.UserNotFound));
        }
        
        var contact = new ContactEntity(command.RequesterId, command.ContactId);

        _context.ContactEntities.Add(contact);
        await _context.SaveChangesAsync();

        var contactDto = new ContactDto(contact.UserId, contact.ContactId, null);
        
        return new Result<ContactDto>(contactDto);
    }
}
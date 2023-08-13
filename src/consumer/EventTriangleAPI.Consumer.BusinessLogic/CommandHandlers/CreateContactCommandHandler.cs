using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public class CreateContactCommandHandler : ICommandHandler<CreateContactCommand, ContactEntity>
{
    private readonly DatabaseContext _context;

    public CreateContactCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<ContactEntity, Error>> HandleAsync(CreateContactCommand command)
    {
        var user = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.UserId);

        if (user == null)
        {
            return new Result<ContactEntity>(new DbEntityNotFoundError("User not found"));
        }
        
        var userForAddingContact = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.ContactId);

        if (userForAddingContact == null)
        {
            return new Result<ContactEntity>(new DbEntityNotFoundError("Contact not found"));
        }
        
        var contact = new ContactEntity(command.UserId, command.ContactId);

        _context.ContactEntities.Add(contact);
        await _context.SaveChangesAsync();

        return new Result<ContactEntity>(contact);
    }
}
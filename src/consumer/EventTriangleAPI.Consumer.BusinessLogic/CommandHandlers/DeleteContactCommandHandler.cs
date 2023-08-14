using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public class DeleteContactCommandHandler : ICommandHandler<DeleteContactCommand, ContactEntity>
{
    private readonly DatabaseContext _context;

    public DeleteContactCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<ContactEntity, Error>> HandleAsync(DeleteContactCommand command)
    {
        var contact = await _context.ContactEntities
            .FirstOrDefaultAsync(x => x.UserId == command.UserId && x.ContactId == command.ContactId);

        if (contact == null)
        {
            return new Result<ContactEntity>(new DbEntityNotFoundError("Contact not found"));
        }

        _context.ContactEntities.Remove(contact);
        await _context.SaveChangesAsync();

        return new Result<ContactEntity>(contact);
    }
}
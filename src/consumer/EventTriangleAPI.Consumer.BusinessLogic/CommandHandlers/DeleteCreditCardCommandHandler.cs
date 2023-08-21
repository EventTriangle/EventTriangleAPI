using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public class DeleteCreditCardCommandHandler : ICommandHandler<DeleteCreditCardCommand, CreditCardEntity>
{
    private readonly DatabaseContext _context;

    public DeleteCreditCardCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<CreditCardEntity, Error>> HandleAsync(DeleteCreditCardCommand command)
    {
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<CreditCardEntity>(new DbEntityNotFoundError("Requester not found"));
        }
        
        var creditCard = await _context.CreditCardEntities
            .FirstOrDefaultAsync(x => x.Id == command.CardId && x.UserId == command.RequesterId);

        if (creditCard == null)
        {
            return new Result<CreditCardEntity>(new DbEntityNotFoundError("Credit card not found"));
        }

        _context.CreditCardEntities.Remove(creditCard);
        await _context.SaveChangesAsync();

        return new Result<CreditCardEntity>(creditCard);
    }
}
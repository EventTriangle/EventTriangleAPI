using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public class AddCreditCardCommandHandler : ICommandHandler<AddCreditCardCommand, CreditCardEntity>
{
    private readonly DatabaseContext _context;

    public AddCreditCardCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<CreditCardEntity, Error>> HandleAsync(AddCreditCardCommand command)
    {
        var user = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.UserId);

        if (user == null)
        {
            return new Result<CreditCardEntity>(new DbEntityNotFoundError("User not found"));
        }

        var countCreditCardWithSameCardNumber = await _context.CreditCardEntities
            .Where(x => x.CardNumber == command.CardNumber && x.UserId == command.UserId)
            .CountAsync();

        if (countCreditCardWithSameCardNumber > 0)
        {
            return new Result<CreditCardEntity>(new BadRequestError("You cannot add 2 credit cards with the same card number"));
        }
        
        var creditCard = new CreditCardEntity(
            command.UserId, 
            command.HolderName, 
            command.CardNumber, 
            command.Cvv,
            command.Expiration,
            command.PaymentNetwork);

        _context.CreditCardEntities.Add(creditCard);
        await _context.SaveChangesAsync();

        return new Result<CreditCardEntity>(creditCard);
    }
}
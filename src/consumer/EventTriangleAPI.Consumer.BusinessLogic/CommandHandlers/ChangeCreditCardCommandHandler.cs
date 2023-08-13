using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public class ChangeCreditCardCommandHandler : ICommandHandler<ChangeCreditCardCommand, CreditCardEntity>
{
    private readonly DatabaseContext _context;

    public ChangeCreditCardCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<CreditCardEntity, Error>> HandleAsync(ChangeCreditCardCommand command)
    {
        var creditCard = await _context.CreditCardEntities
            .FirstOrDefaultAsync(x => x.Id == command.CardId && x.UserId == command.UserId);

        if (creditCard == null)
        {
            return new Result<CreditCardEntity>(new DbEntityNotFoundError("Credit card not found"));
        }

        creditCard.UpdateHolderName(command.HolderName);
        creditCard.UpdateCardNumber(command.CardNumber);
        creditCard.UpdateCvv(command.Cvv);
        creditCard.UpdateExpiration(command.Expiration);
        creditCard.UpdatePaymentNetwork(command.PaymentNetwork);

        _context.CreditCardEntities.Update(creditCard);
        await _context.SaveChangesAsync();

        return new Result<CreditCardEntity>(creditCard);
    }
}
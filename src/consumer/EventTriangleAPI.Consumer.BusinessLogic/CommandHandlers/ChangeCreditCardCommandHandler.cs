using EventTriangleAPI.Consumer.BusinessLogic.Models;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public class ChangeCreditCardCommandHandler : ICommandHandler<ChangeCreditCardCommand, CreditCardDto>
{
    private readonly DatabaseContext _context;

    public ChangeCreditCardCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<CreditCardDto, Error>> HandleAsync(ChangeCreditCardCommand command)
    {
        var creditCard = await _context.CreditCardEntities
            .FirstOrDefaultAsync(x => x.Id == command.CardId && x.UserId == command.RequesterId);

        if (creditCard == null)
        {
            return new Result<CreditCardDto>(new DbEntityNotFoundError(ResponseMessages.CreditCardNotFound));
        }

        creditCard.UpdateHolderName(command.HolderName);
        creditCard.UpdateCardNumber(command.CardNumber);
        creditCard.UpdateCvv(command.Cvv);
        creditCard.UpdateExpiration(command.Expiration);
        creditCard.UpdatePaymentNetwork(command.PaymentNetwork);

        _context.CreditCardEntities.Update(creditCard);
        await _context.SaveChangesAsync();

        var creditCardDto = new CreditCardDto(
            creditCard.Id,
            creditCard.UserId, 
            creditCard.HolderName, 
            creditCard.CardNumber, 
            creditCard.Cvv,
            creditCard.Expiration,
            creditCard.PaymentNetwork);

        return new Result<CreditCardDto>(creditCardDto);
    }
}
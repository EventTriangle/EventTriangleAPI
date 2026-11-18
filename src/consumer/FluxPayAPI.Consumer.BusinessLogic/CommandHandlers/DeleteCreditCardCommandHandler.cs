using FluxPayAPI.Consumer.BusinessLogic.Models;
using FluxPayAPI.Consumer.Domain.Constants;
using FluxPayAPI.Consumer.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;

public class DeleteCreditCardCommandHandler : ICommandHandler<DeleteCreditCardCommand, CreditCardDto>
{
    private readonly DatabaseContext _context;

    public DeleteCreditCardCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<CreditCardDto, Error>> HandleAsync(DeleteCreditCardCommand command)
    {
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<CreditCardDto>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }
        
        var creditCard = await _context.CreditCardEntities
            .FirstOrDefaultAsync(x => x.Id == command.CardId && x.UserId == command.RequesterId);

        if (creditCard == null)
        {
            return new Result<CreditCardDto>(new DbEntityNotFoundError(ResponseMessages.CreditCardNotFound));
        }

        _context.CreditCardEntities.Remove(creditCard);
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
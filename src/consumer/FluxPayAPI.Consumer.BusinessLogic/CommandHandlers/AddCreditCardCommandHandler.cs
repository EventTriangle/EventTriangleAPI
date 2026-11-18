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

public class AddCreditCardCommandHandler : ICommandHandler<AddCreditCardCommand, CreditCardDto>
{
    private readonly DatabaseContext _context;

    public AddCreditCardCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<CreditCardDto, Error>> HandleAsync(AddCreditCardCommand command)
    {
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<CreditCardDto>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }

        var countCreditCardWithSameCardNumber = await _context.CreditCardEntities
            .Where(x => x.CardNumber == command.CardNumber && x.UserId == command.RequesterId)
            .CountAsync();

        if (countCreditCardWithSameCardNumber > 0)
        {
            return new Result<CreditCardDto>(new BadRequestError(ResponseMessages.CannotCreateCreditCardWithSameCardNumber));
        }
        
        var creditCard = new CreditCardEntity(
            command.RequesterId, 
            command.HolderName, 
            command.CardNumber, 
            command.Cvv,
            command.Expiration,
            command.PaymentNetwork);

        _context.CreditCardEntities.Add(creditCard);
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
using EventTriangleAPI.Consumer.BusinessLogic.Models;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public class GetCreditCardsQueryHandler : ICommandHandler<GetCreditCardsQuery, List<CreditCardDto>>
{
    private readonly DatabaseContext _context;

    public GetCreditCardsQueryHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<List<CreditCardDto>, Error>> HandleAsync(GetCreditCardsQuery command)
    {
        var requester = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.RequesterId);

        if (requester == null)
        {
            return new Result<List<CreditCardDto>>(new DbEntityNotFoundError(ResponseMessages.RequesterNotFound));
        }

        var creditCards = await _context.CreditCardEntities
            .Where(x => x.UserId == command.RequesterId)
            .Select(x => new CreditCardDto(
                x.Id,
                x.UserId,
                x.HolderName,
                x.CardNumber,
                x.Cvv,
                x.Expiration,
                x.PaymentNetwork)
            )
            .ToListAsync();

        return new Result<List<CreditCardDto>>(creditCards);
    }
}
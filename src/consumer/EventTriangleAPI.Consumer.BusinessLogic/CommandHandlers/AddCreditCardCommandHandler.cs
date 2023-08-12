using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

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
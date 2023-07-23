using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.Domain.Entities;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class EditCreditCardCommandHandler : ICommandHandler<EditCreditCardCommand, CreditCardChangedEvent>
{
    private readonly DatabaseContext _context;

    public EditCreditCardCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<CreditCardChangedEvent, Error>> HandleAsync(EditCreditCardCommand command)
    {
        var creditCardChangedEvent = new CreditCardChangedEvent(
            command.CardId,
            command.UserId,
            command.HolderName,
            command.CardNumber,
            command.Cvv,
            command.Expiration,
            command.PaymentNetwork);

        _context.CreditCardChangedEvents.Add(creditCardChangedEvent);
        await _context.SaveChangesAsync();
        
        new MockOrder().Send(creditCardChangedEvent);

        return new Result<CreditCardChangedEvent>(creditCardChangedEvent);
    }
}
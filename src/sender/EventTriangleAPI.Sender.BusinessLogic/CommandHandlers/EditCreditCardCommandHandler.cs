using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class EditCreditCardCommandHandler : ICommandHandler<EditCreditCardBody, CreditCardChangedEvent>
{
    private readonly DatabaseContext _context;

    public EditCreditCardCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<CreditCardChangedEvent, Error>> HandleAsync(ICommand<EditCreditCardBody> command)
    {
        var creditCardChangedEvent = new CreditCardChangedEvent(
            command.Body.CardId,
            command.Body.UserId,
            command.Body.HolderName,
            command.Body.CardNumber,
            command.Body.Cvv,
            command.Body.Expiration,
            command.Body.PaymentNetwork);

        _context.CreditCardChangedEvents.Add(creditCardChangedEvent);
        await _context.SaveChangesAsync();
        
        new MockOrder().Send(creditCardChangedEvent);

        return new Result<CreditCardChangedEvent>(creditCardChangedEvent);
    }
}
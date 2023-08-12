using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class AttachCreditCardToAccountCommandHandler : ICommandHandler<AttachCreditCardToAccountCommand, CreditCardAddedEvent>
{
    private readonly DatabaseContext _context;

    public AttachCreditCardToAccountCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<CreditCardAddedEvent, Error>> HandleAsync(AttachCreditCardToAccountCommand command)
    {
        var creditCardAddedEvent = new CreditCardAddedEvent(
            userId: command.UserId,
            command.HolderName,
            command.CardNumber,
            command.Cvv,
            command.Expiration,
            command.PaymentNetwork);

        _context.CreditCardAddedEvents.Add(creditCardAddedEvent);
        await _context.SaveChangesAsync();
        
        new MockOrder().Send(creditCardAddedEvent);
        
        return new Result<CreditCardAddedEvent>(creditCardAddedEvent);
    }
}
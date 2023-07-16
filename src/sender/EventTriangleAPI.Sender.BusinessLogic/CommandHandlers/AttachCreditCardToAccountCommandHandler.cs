using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class AttachCreditCardToAccountCommandHandler : ICommandHandler<AttachCreditCardToAccountBody, string>
{
    private readonly DatabaseContext _context;

    public AttachCreditCardToAccountCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(ICommand<AttachCreditCardToAccountBody> command)
    {
        var creditCardAddedEvent = new CreditCardAddedEvent(
            cardId: Guid.NewGuid(),
            userId: command.Body.UserId,
            command.Body.HolderName,
            command.Body.CardNumber,
            command.Body.Cvv,
            command.Body.Expiration,
            command.Body.PaymentNetwork);

        _context.CreditCardAddedEvents.Add(creditCardAddedEvent);
        await _context.SaveChangesAsync();
        
        new MockOrder().Send(creditCardAddedEvent);
        
        return new Result<string>("");
    }
}
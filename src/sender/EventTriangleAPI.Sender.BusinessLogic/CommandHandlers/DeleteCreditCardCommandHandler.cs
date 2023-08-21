using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class DeleteCreditCardCommandHandler : ICommandHandler<DeleteCreditCardCommand, CreditCardDeletedEvent>
{
    private readonly DatabaseContext _context;

    public DeleteCreditCardCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<CreditCardDeletedEvent, Error>> HandleAsync(DeleteCreditCardCommand command)
    {
        var creditCardDeletedEvent = new CreditCardDeletedEvent(command.RequesterId, command.CardId);

        _context.CreditCardDeletedEvents.Add(creditCardDeletedEvent);
        await _context.SaveChangesAsync();
        
        new MockOrder().Send(creditCardDeletedEvent);

        return new Result<CreditCardDeletedEvent>(creditCardDeletedEvent);
    }
}
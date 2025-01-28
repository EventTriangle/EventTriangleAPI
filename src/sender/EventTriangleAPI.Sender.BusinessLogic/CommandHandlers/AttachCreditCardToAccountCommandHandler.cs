using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class AttachCreditCardToAccountCommandHandler : ICommandHandler<AttachCreditCardToAccountCommand, CreditCardAddedEvent>
{
    private readonly DatabaseContext _context;
    private readonly IPublishEndpoint _client;

    public AttachCreditCardToAccountCommandHandler(DatabaseContext context, IPublishEndpoint client)
    {
        _context = context;
        _client = client;
    }

    public async Task<IResult<CreditCardAddedEvent, Error>> HandleAsync(AttachCreditCardToAccountCommand command)
    {
        var creditCardAddedEvent = new CreditCardAddedEvent(
            command.RequesterId,
            command.HolderName,
            command.CardNumber,
            command.Cvv,
            command.Expiration,
            command.PaymentNetwork);

        _context.CreditCardAddedEvents.Add(creditCardAddedEvent);
        await _context.SaveChangesAsync();
        
        var _ = _client.Publish(creditCardAddedEvent.CreateEventMessage());
        
        return new Result<CreditCardAddedEvent>(creditCardAddedEvent);
    }
}
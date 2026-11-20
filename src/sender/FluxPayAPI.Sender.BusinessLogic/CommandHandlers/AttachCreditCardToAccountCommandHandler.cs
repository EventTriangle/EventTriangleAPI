using FluxPayAPI.Sender.Domain.Entities;
using FluxPayAPI.Sender.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

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
        await _client.Publish(creditCardAddedEvent.CreateEventMessage());
        await _context.SaveChangesAsync();
        
        return new Result<CreditCardAddedEvent>(creditCardAddedEvent);
    }
}
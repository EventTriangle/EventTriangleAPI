using FluxPayAPI.Sender.Domain.Entities;
using FluxPayAPI.Sender.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public class EditCreditCardCommandHandler : ICommandHandler<EditCreditCardCommand, CreditCardChangedEvent>
{
    private readonly DatabaseContext _context;
    private readonly IPublishEndpoint _client;

    public EditCreditCardCommandHandler(DatabaseContext context, IPublishEndpoint client)
    {
        _context = context;
        _client = client;
    }

    public async Task<IResult<CreditCardChangedEvent, Error>> HandleAsync(EditCreditCardCommand command)
    {
        var creditCardChangedEvent = new CreditCardChangedEvent(
            command.CardId,
            command.RequesterId,
            command.HolderName,
            command.CardNumber,
            command.Cvv,
            command.Expiration,
            command.PaymentNetwork);

        _context.CreditCardChangedEvents.Add(creditCardChangedEvent);
        await _client.Publish(creditCardChangedEvent.CreateEventMessage());
        await _context.SaveChangesAsync();

        return new Result<CreditCardChangedEvent>(creditCardChangedEvent);
    }
}
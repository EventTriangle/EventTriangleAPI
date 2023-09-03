using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

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
        await _context.SaveChangesAsync();
        
        var _ = _client.Publish(creditCardChangedEvent.CreateEventMessage());

        return new Result<CreditCardChangedEvent>(creditCardChangedEvent);
    }
}
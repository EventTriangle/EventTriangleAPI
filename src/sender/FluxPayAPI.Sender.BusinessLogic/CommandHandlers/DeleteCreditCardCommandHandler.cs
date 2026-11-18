using FluxPayAPI.Sender.Domain.Entities;
using FluxPayAPI.Sender.Persistence;
using FluxPayAPI.Shared.Application.Abstractions;
using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses;
using FluxPayAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public class DeleteCreditCardCommandHandler : ICommandHandler<DeleteCreditCardCommand, CreditCardDeletedEvent>
{
    private readonly DatabaseContext _context;
    private readonly IPublishEndpoint _client;

    public DeleteCreditCardCommandHandler(DatabaseContext context, IPublishEndpoint client)
    {
        _context = context;
        _client = client;
    }

    public async Task<IResult<CreditCardDeletedEvent, Error>> HandleAsync(DeleteCreditCardCommand command)
    {
        var creditCardDeletedEvent = new CreditCardDeletedEvent(command.RequesterId, command.CardId);

        _context.CreditCardDeletedEvents.Add(creditCardDeletedEvent);
        await _context.SaveChangesAsync();
        
        var _ = _client.Publish(creditCardDeletedEvent.CreateEventMessage());

        return new Result<CreditCardDeletedEvent>(creditCardDeletedEvent);
    }
}
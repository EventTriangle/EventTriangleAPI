using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using MassTransit;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

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
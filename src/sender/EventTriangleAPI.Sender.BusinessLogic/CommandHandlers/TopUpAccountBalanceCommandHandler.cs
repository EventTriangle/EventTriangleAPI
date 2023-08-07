using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class TopUpAccountBalanceCommandHandler : ICommandHandler<TopUpAccountBalanceCommand, TransactionCardToUserCreatedEvent>
{
    private readonly DatabaseContext _context;

    public TopUpAccountBalanceCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<TransactionCardToUserCreatedEvent, Error>> HandleAsync(TopUpAccountBalanceCommand command)
    {
        var transactionCreatedEvent = new TransactionCardToUserCreatedEvent(
            command.CreditCardId,
            command.ToUserId,
            command.Amount);

        _context.TransactionCardToUserCreatedEvents.Add(transactionCreatedEvent);
        await _context.SaveChangesAsync();
        
        new MockOrder().Send(transactionCreatedEvent);

        return new Result<TransactionCardToUserCreatedEvent>(transactionCreatedEvent);
    }
}
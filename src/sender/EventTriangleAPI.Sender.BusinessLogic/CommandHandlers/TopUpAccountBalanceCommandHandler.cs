using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public class TopUpAccountBalanceCommandHandler : ICommandHandler<TopUpAccountBalanceBody, string>
{
    private readonly DatabaseContext _context;

    public TopUpAccountBalanceCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<string, Error>> HandleAsync(ICommand<TopUpAccountBalanceBody> command)
    {
        var transactionCreatedEvent = new TransactionCreatedEvent(
            command.Body.From,
            command.Body.To,
            command.Body.Amount,
            command.Body.TransactionType);

        _context.TransactionCreatedEvents.Add(transactionCreatedEvent);
        await _context.SaveChangesAsync();
        
        new MockOrder().Send(transactionCreatedEvent);

        return new Result<string>("");
    }
}
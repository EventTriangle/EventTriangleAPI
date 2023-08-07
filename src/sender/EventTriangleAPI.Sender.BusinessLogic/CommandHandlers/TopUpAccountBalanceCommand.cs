using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record TopUpAccountBalanceCommand(
    Guid CreditCardId, 
    string ToUserId,
    decimal Amount) : ICommand;

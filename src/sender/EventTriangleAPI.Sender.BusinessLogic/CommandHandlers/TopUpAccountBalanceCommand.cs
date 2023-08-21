using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record TopUpAccountBalanceCommand(
    string RequesterId,
    Guid CreditCardId,
    decimal Amount) : ICommand;

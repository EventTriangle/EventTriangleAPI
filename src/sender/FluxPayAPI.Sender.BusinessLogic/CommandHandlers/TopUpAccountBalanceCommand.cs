using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public record TopUpAccountBalanceCommand(
    string RequesterId,
    Guid CreditCardId,
    decimal Amount) : ICommand;

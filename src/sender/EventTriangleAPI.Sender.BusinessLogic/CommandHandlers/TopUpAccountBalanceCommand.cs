using EventTriangleAPI.Shared.Domain.Enums;
using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record TopUpAccountBalanceCommand(
    string From, 
    string To,
    decimal Amount,
    TransactionType TransactionType) : ICommand;

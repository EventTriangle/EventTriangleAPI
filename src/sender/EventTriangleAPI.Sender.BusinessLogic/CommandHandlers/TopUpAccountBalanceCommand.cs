using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record TopUpAccountBalanceCommand(
    string From, 
    string To,
    decimal Amount,
    TransactionType TransactionType) : ICommand;

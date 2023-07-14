using EventTriangleAPI.Shared.Application.Enums;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record TopUpAccountBalanceBody(
    string From, 
    string To,
    decimal Amount,
    TransactionType TransactionType);

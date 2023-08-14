using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record RollBackTransactionCommand(string RequesterId, Guid TransactionId) : ICommand;
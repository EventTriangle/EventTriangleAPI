using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public record RollBackTransactionCommand(string RequesterId, Guid TransactionId) : ICommand;
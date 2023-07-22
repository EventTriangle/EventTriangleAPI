using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record RollBackTransactionCommand(Guid TransactionId) : ICommand;
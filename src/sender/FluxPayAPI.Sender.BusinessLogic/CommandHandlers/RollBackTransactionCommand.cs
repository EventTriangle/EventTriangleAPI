using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public record RollBackTransactionCommand(string RequesterId, Guid TransactionId) : ICommand;
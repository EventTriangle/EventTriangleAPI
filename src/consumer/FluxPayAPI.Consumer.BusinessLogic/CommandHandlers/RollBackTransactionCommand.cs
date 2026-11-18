using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;

public record RollBackTransactionCommand(string RequesterId, Guid TransactionId) : ICommand;
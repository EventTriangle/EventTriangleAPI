using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public record CreateTransactionUserToUserCommand(
    string RequesterId, 
    string ToUserId,
    decimal Amount, Guid? OperationId = null) : ICommand;
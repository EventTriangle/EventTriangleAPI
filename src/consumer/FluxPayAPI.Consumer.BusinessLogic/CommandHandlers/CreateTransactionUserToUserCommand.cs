using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;

public record CreateTransactionUserToUserCommand(
    string RequesterId, 
    string ToUserId, 
    decimal Amount,
    DateTime CreatedAt) : ICommand;
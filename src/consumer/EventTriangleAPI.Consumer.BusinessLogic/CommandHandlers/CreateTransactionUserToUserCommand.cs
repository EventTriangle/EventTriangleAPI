using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public record CreateTransactionUserToUserCommand(
    string RequesterId, 
    string ToUserId, 
    decimal Amount,
    DateTime CreatedAt) : ICommand;
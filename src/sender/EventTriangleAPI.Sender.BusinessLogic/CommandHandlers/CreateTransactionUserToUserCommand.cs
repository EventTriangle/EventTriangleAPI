using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record CreateTransactionUserToUserCommand(
    string RequesterId, 
    string ToUserId,
    decimal Amount) : ICommand;
using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record CreateTransactionUserToUserCommand(
    string FromUserId, 
    string ToUserId,
    decimal Amount) : ICommand;
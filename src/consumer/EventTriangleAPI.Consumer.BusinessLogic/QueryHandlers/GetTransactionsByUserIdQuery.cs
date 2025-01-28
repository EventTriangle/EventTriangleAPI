using ICommand = EventTriangleAPI.Shared.DTO.Abstractions.ICommand;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetTransactionsByUserIdQuery(
    string RequesterId, 
    string UserId, 
    int Limit,
    DateTime FromDateTime) : ICommand;
using ICommand = FluxPayAPI.Shared.DTO.Abstractions.ICommand;

namespace FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetTransactionsByUserIdQuery(
    string RequesterId, 
    string UserId, 
    int Limit,
    DateTime FromDateTime) : ICommand;
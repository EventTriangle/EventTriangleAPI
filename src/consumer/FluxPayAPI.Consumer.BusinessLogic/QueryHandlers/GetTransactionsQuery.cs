using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetTransactionsQuery(
    string RequesterId, 
    int Limit, 
    DateTime FromDateTime) : ICommand;
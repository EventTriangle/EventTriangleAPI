using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetTransactionsBySearchQuery(
    string RequesterId,
    string SearchText,
    int Limit, 
    DateTime FromDateTime) : ICommand;
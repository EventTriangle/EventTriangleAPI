using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetTransactionsBySearchQuery(
    string RequesterId,
    string SearchText,
    int Limit, 
    DateTime FromDateTime) : ICommand;
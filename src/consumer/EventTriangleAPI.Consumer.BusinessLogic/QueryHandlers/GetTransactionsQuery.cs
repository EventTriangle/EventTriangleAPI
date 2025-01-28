using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetTransactionsQuery(
    string RequesterId, 
    int Limit, 
    DateTime FromDateTime) : ICommand;
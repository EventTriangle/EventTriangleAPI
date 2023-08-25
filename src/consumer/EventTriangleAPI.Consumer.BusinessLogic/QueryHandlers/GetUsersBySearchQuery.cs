using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetUsersBySearchQuery(
    string RequesterId, 
    string Email,
    int Limit, 
    int Page) : ICommand;
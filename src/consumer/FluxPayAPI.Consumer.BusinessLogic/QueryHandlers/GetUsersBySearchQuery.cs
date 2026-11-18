using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetUsersBySearchQuery(
    string RequesterId, 
    string Email,
    int Limit, 
    int Page) : ICommand;
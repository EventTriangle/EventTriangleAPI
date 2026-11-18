using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetUsersQuery(string RequesterId, int Limit, int Page) : ICommand;
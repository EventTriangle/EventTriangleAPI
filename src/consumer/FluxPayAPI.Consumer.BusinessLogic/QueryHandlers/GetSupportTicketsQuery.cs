using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetSupportsTicketsQuery(string RequesterId, int Limit, DateTime FromDateTime, Guid? CursorId = null) : ICommand;
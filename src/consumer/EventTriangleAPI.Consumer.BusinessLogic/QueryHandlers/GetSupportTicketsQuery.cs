using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetSupportsTicketsQuery(string RequesterId, int Limit, DateTime FromDateTime) : ICommand;
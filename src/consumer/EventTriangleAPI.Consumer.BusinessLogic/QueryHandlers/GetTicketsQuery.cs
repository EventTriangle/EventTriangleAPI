using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetTicketsQuery(string RequesterId) : ICommand;
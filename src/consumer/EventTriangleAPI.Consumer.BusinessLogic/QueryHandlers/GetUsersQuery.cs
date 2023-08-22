using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetUsersQuery(string RequesterId, int Limit, int Page) : ICommand;
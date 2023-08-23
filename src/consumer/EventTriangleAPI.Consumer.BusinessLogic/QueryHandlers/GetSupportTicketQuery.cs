using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetSupportTicketQuery(string RequesterId, int Limit, DateTime FromDateTime) : ICommand;
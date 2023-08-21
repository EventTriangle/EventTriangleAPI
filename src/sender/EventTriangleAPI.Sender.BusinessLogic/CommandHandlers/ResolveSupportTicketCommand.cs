using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record ResolveSupportTicketCommand(string RequesterId, Guid TicketId, string TicketJustification) : ICommand;
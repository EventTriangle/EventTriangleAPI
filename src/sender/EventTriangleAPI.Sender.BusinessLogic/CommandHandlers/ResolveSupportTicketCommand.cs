using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record ResolveSupportTicketCommand(Guid TicketId, string TicketJustification) : ICommand;
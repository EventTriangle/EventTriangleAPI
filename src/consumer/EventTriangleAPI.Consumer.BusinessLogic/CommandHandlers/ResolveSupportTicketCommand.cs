using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public record ResolveSupportTicketCommand(Guid TicketId, string TicketJustification) : ICommand;
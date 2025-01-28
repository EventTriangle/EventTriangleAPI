using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public record ResolveSupportTicketCommand(
    string RequesterId, 
    Guid TicketId, 
    string TicketJustification) : ICommand;
namespace EventTriangleAPI.Shared.DTO.Messages;

public record SupportTicketResolvedEventMessage(
    Guid Id,
    string RequesterId,
    Guid TicketId, 
    string TicketJustification, 
    DateTime CreatedAt);
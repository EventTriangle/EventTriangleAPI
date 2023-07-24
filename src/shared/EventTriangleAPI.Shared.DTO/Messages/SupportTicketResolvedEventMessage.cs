namespace EventTriangleAPI.Shared.DTO.Messages;

public record SupportTicketResolvedEventMessage(Guid Id, Guid TicketId, string TicketJustification, DateTime CreatedAt);
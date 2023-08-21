namespace EventTriangleAPI.Shared.DTO.Messages;

public record SupportTicketOpenedEventMessage(
    Guid Id, 
    string RequesterId, 
    Guid WalletId, 
    string TicketReason, 
    DateTime CreatedAt);
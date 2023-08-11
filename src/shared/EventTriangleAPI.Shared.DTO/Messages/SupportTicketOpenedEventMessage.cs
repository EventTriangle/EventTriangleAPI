namespace EventTriangleAPI.Shared.DTO.Messages;

public record SupportTicketOpenedEventMessage(
    Guid Id, 
    string UserId, 
    Guid WalletId, 
    string TicketReason, 
    DateTime CreatedAt);
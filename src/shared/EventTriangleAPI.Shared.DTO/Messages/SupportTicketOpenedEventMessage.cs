namespace EventTriangleAPI.Shared.DTO.Messages;

public record SupportTicketOpenedEventMessage(
    Guid Id, 
    string UserId, 
    string Username, 
    Guid WalletId, 
    string TicketReason, 
    DateTime CreatedAt);
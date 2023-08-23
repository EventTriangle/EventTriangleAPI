namespace EventTriangleAPI.Shared.DTO.Messages;

public record SupportTicketOpenedEventMessage(
    Guid Id, 
    string RequesterId, 
    Guid WalletId, 
    Guid TransactionId,
    string TicketReason, 
    DateTime CreatedAt);
namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record OpenSupportTicketBody(
    Guid RequesterId, 
    string Username,
    Guid WalletId,
    string TicketReason);
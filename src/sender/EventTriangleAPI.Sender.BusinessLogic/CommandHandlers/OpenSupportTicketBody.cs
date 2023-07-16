namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record OpenSupportTicketBody(
    string UserId, 
    string Username,
    Guid WalletId,
    string TicketReason);
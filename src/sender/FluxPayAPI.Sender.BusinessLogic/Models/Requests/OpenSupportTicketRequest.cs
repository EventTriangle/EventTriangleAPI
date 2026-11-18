namespace FluxPayAPI.Sender.BusinessLogic.Models.Requests;

public record OpenSupportTicketRequest(
    Guid WalletId,
    Guid TransactionId,
    string TicketReason);

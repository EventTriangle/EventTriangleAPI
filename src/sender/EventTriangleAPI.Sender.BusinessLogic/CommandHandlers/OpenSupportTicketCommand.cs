using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;

public record OpenSupportTicketCommand(
    string RequesterId, 
    Guid WalletId,
    Guid TransactionId,
    string TicketReason) : ICommand;
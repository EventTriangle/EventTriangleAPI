using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Sender.BusinessLogic.CommandHandlers;

public record OpenSupportTicketCommand(
    string RequesterId, 
    Guid WalletId,
    Guid TransactionId,
    string TicketReason) : ICommand;
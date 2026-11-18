using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.CommandHandlers;

public record OpenSupportTicketCommand(
    string RequesterId, 
    Guid WalletId,
    Guid TransactionId,
    string TicketReason,
    DateTime CreatedAt) : ICommand;
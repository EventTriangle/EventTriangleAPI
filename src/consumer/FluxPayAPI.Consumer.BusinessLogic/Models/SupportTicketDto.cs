using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Consumer.BusinessLogic.Models;

public record SupportTicketDto(
    Guid Id,
    Guid TransactionId,
    string UserId,
    Guid WalletId,
    string TicketReason,
    string TicketJustification,
    TicketStatus TicketStatus,
    DateTime CreatedAt);

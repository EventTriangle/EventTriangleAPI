using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.BusinessLogic.Models;

public record SupportTicketDto(
    Guid Id,
    string UserId,
    Guid WalletId,
    string TicketReason,
    string TicketJustification,
    TicketStatus TicketStatus);

using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.BusinessLogic.Models.Notifications;

public record TransactionCanceledNotification(
    string FromUserId,
    string ToUserId,
    decimal Amount,
    TransactionType TransactionType,
    DateTime CreatedAt,
    string Reason
);
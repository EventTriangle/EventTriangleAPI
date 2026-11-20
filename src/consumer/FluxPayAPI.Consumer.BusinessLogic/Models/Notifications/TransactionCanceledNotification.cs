using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Consumer.BusinessLogic.Models.Notifications;

public record TransactionCanceledNotification(
    string FromUserId,
    string ToUserId,
    decimal Amount,
    TransactionType TransactionType,
    DateTime CreatedAt,
    string Reason,
    Guid? OperationId = null);
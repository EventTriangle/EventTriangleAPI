using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.BusinessLogic.Models;

public record TransactionDto(
    Guid Id, 
    string FromUserId,
    string ToUserId,
    decimal Amount,
    TransactionState TransactionState,
    TransactionType TransactionType,
    DateTime CreatedAt);
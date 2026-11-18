using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Consumer.BusinessLogic.Models;

public record TransactionDto(
    Guid Id, 
    string FromUserId,
    string ToUserId,
    decimal Amount,
    TransactionState TransactionState,
    TransactionType TransactionType,
    DateTime CreatedAt);
using EventTriangleAPI.Shared.Application.Enums;

namespace EventTriangleAPI.Shared.DTO.Messages;

public record TransactionCreatedEventMessage(
    Guid Id, 
    string From, 
    string To,
    decimal Amount,
    TransactionType TransactionType,
    DateTime CreatedAt);
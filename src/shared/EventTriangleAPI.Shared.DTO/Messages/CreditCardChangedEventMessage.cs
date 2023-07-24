using EventTriangleAPI.Shared.Application.Enums;

namespace EventTriangleAPI.Shared.DTO.Messages;

public record CreditCardChangedEventMessage(
    Guid Id,
    Guid CardId, 
    string UserId,
    string HolderName, 
    string CardNumber,
    string Cvv,
    string Expiration,
    PaymentNetwork PaymentNetwork,
    DateTime CreatedAt);
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Shared.DTO.Messages;

public record CreditCardAddedEventMessage(
    Guid Id,
    string UserId,
    string HolderName, 
    string CardNumber,
    string Cvv,
    string Expiration,
    PaymentNetwork PaymentNetwork,
    DateTime CreatedAt);
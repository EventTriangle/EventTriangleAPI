using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Shared.DTO.Messages;

public record CreditCardAddedEventMessage(
    Guid Id,
    Guid CardId, 
    string UserId,
    string HolderName, 
    string CardNumber,
    string Cvv,
    string Expiration,
    PaymentNetwork PaymentNetwork,
    DateTime CreatedAt);
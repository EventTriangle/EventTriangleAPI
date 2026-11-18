using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Shared.DTO.Messages;

public record CreditCardAddedEventMessage(
    Guid Id,
    string RequesterId,
    string HolderName, 
    string CardNumber,
    string Cvv,
    string Expiration,
    PaymentNetwork PaymentNetwork,
    DateTime CreatedAt);